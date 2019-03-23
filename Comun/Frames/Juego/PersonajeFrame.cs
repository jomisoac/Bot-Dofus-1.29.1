using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class PersonajeFrame : Frame
    {
        [PaqueteAtributo("AR")]
        public void get_Restricciones(ClienteGame cliente, string paquete) => cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;

        [PaqueteAtributo("As")]
        public void get_Stats_Actualizados(ClienteGame cliente, string paquete) => cliente.cuenta.personaje.actualizar_Caracteristicas(paquete);

        [PaqueteAtributo("PIK")]
        public void get_Peticion_Grupo(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("Grupo", "Nueva invitación de grupo del personaje: " + paquete.Substring(3).Split('|')[0]);
            cliente.enviar_Paquete("PR");
            cliente.cuenta.logger.log_informacion("Grupo", "Petición rechazada");
        }

        [PaqueteAtributo("SL")]
        public void get_Lista_Hechizos(ClienteGame cliente, string paquete)
        {
            if (!paquete[2].Equals('o'))
                cliente.cuenta.personaje.actualizar_Hechizos(paquete.Substring(2));
        }

        [PaqueteAtributo("Ow")]
        public void get_Actualizacion_Pods(ClienteGame cliente, string paquete)
        {
            string[] pods = paquete.Substring(2).Split('|');
            short pods_actuales = short.Parse(pods[0]);
            short pods_maximos = short.Parse(pods[1]);

            cliente.cuenta.personaje.inventario.pods_actuales = pods_actuales;
            cliente.cuenta.personaje.inventario.pods_maximos = pods_maximos;
        }
    }
}
