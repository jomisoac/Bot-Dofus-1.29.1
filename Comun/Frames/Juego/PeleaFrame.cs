using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
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
    class PeleaFrame : Frame
    {
        [PaqueteAtributo("GP")]
        public async Task get_Combate_Celdas_Posicion(ClienteAbstracto cliente, string paquete)
        {
            var _loc3 = paquete.Substring(2).Split('|');
            string _loc4 = _loc3[0];
            string _loc5 = _loc3[1];
            int equipo_actual = int.Parse(_loc3[2]);
            Cuenta cuenta = cliente.cuenta;

            if (cuenta.pelea_extension.configuracion.desactivar_espectador)
                await cuenta.conexion.enviar_Paquete("fS");

            if(cuenta.puede_utilizar_dragopavo)
            {
                if (cuenta.pelea_extension.configuracion.utilizar_dragopavo && !cuenta.personaje.esta_utilizando_dragopavo)
                {
                    await cuenta.conexion.enviar_Paquete("Rr");
                    cuenta.personaje.esta_utilizando_dragopavo = true;
                }
            }

            cuenta.pelea.get_Combate_Creado();
            await cuenta.conexion.enviar_Paquete("GR1");//boton listo
        }

        [PaqueteAtributo("GTM")]
        public void get_Combate_Info_Stats(ClienteAbstracto cliente, string paquete)
        {
            string[] separador = paquete.Substring(4).Split('|');

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
                        int celda_id = int.Parse(_loc6_[5]);
                        int vida_maxima = int.Parse(_loc6_[7]);

                        if (celda_id > 0)//son espectadores
                        {
                            byte equipo = Convert.ToByte(id > 0 ? 1 : 0);
                            luchador?.get_Actualizar_Luchador(id, esta_vivo, vida_actual, pa, pm, celda_id, vida_maxima, equipo);
                        }
                    }
                    else
                        luchador?.get_Actualizar_Luchador(id, esta_vivo, 0, 0, 0, -1, 0, 0);
                }
            }
        }

        [PaqueteAtributo("GTR")]
        public async Task get_Combate_Turno_Listo(ClienteAbstracto cliente, string paquete) => await cliente.cuenta.conexion.enviar_Paquete("GT");

        [PaqueteAtributo("GTS")]
        public void get_Combate_Inicio_Turno(ClienteAbstracto cliente, string paquete)
        {
            if (int.Parse(paquete.Substring(3).Split('|')[0]) == cliente.cuenta.personaje.id)
                cliente.cuenta.pelea.get_Turno_Iniciado();
        }

        [PaqueteAtributo("GE")]
        public async Task get_Combate_Finalizado(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.pelea.get_Combate_Finalizado();
            await cliente.cuenta.conexion.enviar_Paquete("BD");
        }
    }
}
