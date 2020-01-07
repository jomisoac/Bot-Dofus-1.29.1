using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class FightFrame : Frame
    {
        [PaqueteAtributo("GP")]
        public void get_Combate_Celdas_Posicion(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            Map mapa = cuenta.game.map;
            string[] _loc3 = paquete.Substring(2).Split('|');

            for (int a = 0; a < _loc3[0].Length; a += 2)
                cuenta.game.fight.celdas_preparacion.Add(mapa.GetCellFromId((short)((Hash.get_Hash(_loc3[0][a]) << 6) + Hash.get_Hash(_loc3[0][a + 1]))));
                
            if (cuenta.fightExtension.configuracion.desactivar_espectador)
                cliente.SendPacket("fS");

            if (cuenta.canUseMount)
            {
                if (cuenta.fightExtension.configuracion.utilizar_dragopavo && !cuenta.game.character.esta_utilizando_dragopavo)
                {
                    cliente.SendPacket("Rr");
                    cuenta.game.character.esta_utilizando_dragopavo = true;
                }
            }
        }

        [PaqueteAtributo("GICE")]
        public async Task get_Error_Cambiar_Pos_Pelea(TcpClient cliente, string paquete)
        {
            if(cliente.account.IsFighting())
            {
                await Task.Delay(2850);
                cliente.SendPacket("GR1");//boton listo
            }
        }

        [PaqueteAtributo("GIC")]
        public async Task get_Cambiar_Pos_Pelea(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            string[] separador_posiciones = paquete.Substring(4).Split('|');
            int id_entidad;
            short celda;
            Map mapa = cuenta.game.map;

            foreach (string posicion in separador_posiciones)
            {
                id_entidad = int.Parse(posicion.Split(';')[0]);
                celda = short.Parse(posicion.Split(';')[1]);

                if (id_entidad == cuenta.game.character.id )
                {
                    await Task.Delay(2850);
                    cliente.SendPacket("GR1");//boton listo
                }

                Luchadores luchador = cuenta.game.fight.get_Luchador_Por_Id(id_entidad);
                if (luchador != null)
                    luchador.celda = mapa.GetCellFromId(celda);
            }
        }

        [PaqueteAtributo("GTM")]
        public void get_Combate_Info_Stats(TcpClient cliente, string paquete)
        {
            string[] separador = paquete.Substring(4).Split('|');
            Map mapa = cliente.account.game.map;

            for (int i = 0; i < separador.Length; ++i)
            {
                string[] _loc6_ = separador[i].Split(';');
                int id = int.Parse(_loc6_[0]);
                Luchadores luchador = cliente.account.game.fight.get_Luchador_Por_Id(id);

                if (_loc6_.Length != 0)
                {
                    bool esta_vivo = _loc6_[1].Equals("0");
                    if (esta_vivo)
                    {
                        int vida_actual = int.Parse(_loc6_[2]);
                        byte pa = byte.Parse(_loc6_[3]);
                        byte pm = byte.Parse(_loc6_[4]);
                        short celda = short.Parse(_loc6_[5]);
                        int vida_maxima = int.Parse(_loc6_[7]);

                        if (celda > 0)//son espectadores
                        {
                            byte equipo = Convert.ToByte(id > 0 ? 1 : 0);
                            if (luchador == null)
                            {
                                luchador = new Luchadores(id, esta_vivo, vida_actual, pa, pm, mapa.GetCellFromId(celda), vida_maxima, equipo);
                                cliente.account.game.fight.get_Agregar_Luchador(luchador);
                            }
                            luchador?.get_Actualizar_Luchador(id, esta_vivo, vida_actual, pa, pm, mapa.GetCellFromId(celda), vida_maxima, equipo);
                        }
                    }
                    else
                        luchador?.get_Actualizar_Luchador(id, esta_vivo, 0, 0, 0, null, 0, 0);
                }
            }
        }

        [PaqueteAtributo("GTR")]
        public void get_Combate_Turno_Listo(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            int id = int.Parse(paquete.Substring(3));

            if(cuenta.game.character.id == id)
                cuenta.connexion.SendPacket("BD");

            cuenta.connexion.SendPacket("GT");
        }

        [PaqueteAtributo("GJK")]
        public async void get_Combate_Unirse_Pelea(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            //GJK - estado|boton_cancelar|mostrat_botones|espectador|tiempo|tipo_pelea
            string[] separador = paquete.Substring(3).Split('|');
            byte estado_pelea = byte.Parse(separador[0]);

            switch (estado_pelea)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    if (cuenta.isGroupLeader == true)
                    {
                        cuenta.game.fight.Block_OnlyForgroupe();
                    }
                    await cuenta.game.fight.FightCreatedAsync();
               break;
            }
        }

        [PaqueteAtributo("GTS")]
        public void get_Fight_Start_Turn(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            if (int.Parse(paquete.Substring(3).Split('|')[0]) != cuenta.game.character.id || cuenta.game.fight.total_enemigos_vivos <= 0)
                return;


            cuenta.game.fight.get_Turno_Iniciado();
        }

        [PaqueteAtributo("GE")]
        public async Task get_End_FightAsync(TcpClient cliente, string paquete)
        {
            string[] ListProp = paquete.Substring(2).Split('|');

            int fightTime = Int32.Parse(ListProp[0]);
            int xp = 0;
            string FightTimeConverted = string.Empty;
            Account account = cliente.account;

            foreach (var item in ListProp)
            {


                if (item.Contains(account.game.character.nombre))
                {
                    string[] ListProp2 = item.Split(';');
                     xp = Int32.Parse(ListProp2[8]);

                    TimeSpan t = TimeSpan.FromMilliseconds(fightTime);
                     FightTimeConverted = string.Format("{0:D2}m:{1:D2}s",
                                            t.Minutes,
                                            t.Seconds);
                }
            }

            account.game.fight.Fightfinished(xp, FightTimeConverted);
            cliente.SendPacket("GC1");

            if(account.isGroupLeader)
            {
                account.Logger.LogInfo("Fight", "Attente de 1500 ms");
                await Task.Delay(1500);
            }

        }


    }
}
