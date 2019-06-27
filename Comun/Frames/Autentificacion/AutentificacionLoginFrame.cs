using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using System.Text;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Autentificacion
{
    class AutentificacionLogin : Frame
    {
        [PaqueteAtributo("AlEf")]
        public void get_Error_Datos(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("LOGIN", "Conexión rechazada. Nombre de cuenta o contraseña incorrectos.");
            cliente.cuenta.desconectar();
        }

        [PaqueteAtributo("AlEa")]
        public void get_Error_Ya_Conectado(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("LOGIN", "Ya conectado. Inténtalo de nuevo.");
            cliente.cuenta.desconectar();
        }

        [PaqueteAtributo("AlEv")]
        public void get_Error_Version(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("LOGIN", "La versión %1 de Dofus que tienes instalada no es compatible con este servidor. Para poder jugar, instala la versión %2. El cliente DOFUS se va a cerrar.");
            cliente.cuenta.desconectar();
        }

        [PaqueteAtributo("AlEb")]
        public void get_Error_Baneado(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("LOGIN", "Conexión rechazada. Tu cuenta ha sido baneada.");
            cliente.cuenta.desconectar();
        }

        [PaqueteAtributo("AlEd")]
        public void get_Error_Conectado(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("LOGIN", "Esta cuenta ya está conectada a un servidor de juego. Por favor, inténtalo de nuevo.");
            cliente.cuenta.desconectar();
        }

        [PaqueteAtributo("AlEk")]
        public void get_Error_Baneado_Tiempo(ClienteTcp cliente, string paquete)
        {
            string[] informacion_ban = paquete.Substring(3).Split('|');
            int dias = int.Parse(informacion_ban[0].Substring(1)), horas = int.Parse(informacion_ban[1]), minutos = int.Parse(informacion_ban[2]);
            StringBuilder mensaje = new StringBuilder().Append("Tu cuenta estará inválida durante ");

            if (dias > 0)
                mensaje.Append(dias + " días");
            if (horas > 0)
                mensaje.Append(horas + " con horas");
            if (minutos > 0)
                mensaje.Append(minutos + " y minutos");

            cliente.cuenta.logger.log_Error("LOGIN", mensaje.ToString());
            cliente.cuenta.desconectar();
        }
    }
}
