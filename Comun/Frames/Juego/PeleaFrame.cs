using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
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
    internal class PeleaFrame : Frame
    {
        [PaqueteAtributo("GP")]
        public void get_Combate_Celdas_Posicion(ClienteAbstracto cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] _loc3 = paquete.Substring(2).Split('|');

            for (int a = 0; a < _loc3[0].Length; a += 2)
                cuenta.pelea.celdas_preparacion.Add((short)((Hash.get_Hash(_loc3[0][a]) << 6) + Hash.get_Hash(_loc3[0][a + 1])));
                
            if (cuenta.pelea_extension.configuracion.desactivar_espectador)
               cuenta.conexion.enviar_Paquete("fS");

            if (cuenta.puede_utilizar_dragopavo)
            {
                if (cuenta.pelea_extension.configuracion.utilizar_dragopavo && !cuenta.personaje.esta_utilizando_dragopavo)
                {
                    cuenta.conexion.enviar_Paquete("Rr");
                    cuenta.personaje.esta_utilizando_dragopavo = true;
                }
            }
            cuenta.pelea.get_Combate_Creado();
        }

        [PaqueteAtributo("GICE")]
        public async Task get_Error_Cambiar_Pos_Pelea(ClienteAbstracto cliente, string paquete)
        {
            await Task.Delay(300);
            cliente.cuenta.conexion.enviar_Paquete("GR1");//boton listo
        }

        [PaqueteAtributo("GIC")]
        public async Task get_Cambiar_Pos_Pelea(ClienteAbstracto cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] separador_posiciones = paquete.Substring(4).Split('|');
            int id_entidad;
            Celda celda;
            Luchadores luchador = null;

            foreach (string posicion in separador_posiciones)
            {
                id_entidad = int.Parse(posicion.Split(';')[0]);
                celda = cuenta.personaje.mapa.celdas[short.Parse(posicion.Split(';')[1])];

                if (id_entidad == cuenta.personaje.id)
                {
                    await Task.Delay(300);
                    cuenta.conexion.enviar_Paquete("GR1");//boton listo
                }

                luchador = cuenta.pelea.get_Luchador_Por_Id(id_entidad);

                if (luchador != null)
                    luchador.celda = celda;
            }
        }

        [PaqueteAtributo("GTM")]
        public void get_Combate_Info_Stats(ClienteAbstracto cliente, string paquete)
        {
            string[] separador = paquete.Substring(4).Split('|');
            Mapa mapa = cliente.cuenta.personaje.mapa;

            for (int i = 0; i < separador.Length; ++i)
            {
                string[] _loc6_ = separador[i].Split(';');
                int id = int.Parse(_loc6_[0]);
                Luchadores luchador = cliente.cuenta.pelea.get_Luchador_Por_Id(id);

                if (_loc6_.Length != 0)
                {
                    bool esta_vivo = _loc6_[1].Equals("0");
                    if (esta_vivo)
                    {
                        int vida_actual = int.Parse(_loc6_[2]);
                        byte pa = byte.Parse(_loc6_[3]);
                        byte pm = byte.Parse(_loc6_[4]);
                        Celda celda = mapa.celdas[short.Parse(_loc6_[5])];
                        int vida_maxima = int.Parse(_loc6_[7]);

                        if (celda.id > 0)//son espectadores
                        {
                            byte equipo = Convert.ToByte(id > 0 ? 1 : 0);
                            luchador?.get_Actualizar_Luchador(id, esta_vivo, vida_actual, pa, pm, celda, vida_maxima, equipo);
                        }
                    }
                    else
                        luchador?.get_Actualizar_Luchador(id, esta_vivo, 0, 0, 0, null, 0, 0);
                }
            }
        }

        [PaqueteAtributo("GTR")]
        public void get_Combate_Turno_Listo(ClienteAbstracto cliente, string paquete) => cliente.cuenta.conexion.enviar_Paquete("GT");

        [PaqueteAtributo("GTS")]
        public void get_Combate_Inicio_Turno(ClienteAbstracto cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;

            if (int.Parse(paquete.Substring(3).Split('|')[0]) == cuenta.personaje.id && cliente.cuenta.pelea.total_enemigos_vivos > 0)
                cuenta.pelea.get_Turno_Iniciado();
        }

        [PaqueteAtributo("GE")]
        public void get_Combate_Finalizado(ClienteAbstracto cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;

            cuenta.pelea.get_Combate_Finalizado();
            cuenta.conexion.enviar_Paquete("BD");
        }
    }
}
