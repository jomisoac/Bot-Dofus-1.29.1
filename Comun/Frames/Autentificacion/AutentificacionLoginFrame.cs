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
        public async void get_Error_Datos(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("Login", "Conexión rechazada. Nombre de cuenta o contraseña incorrectos.");
           await cliente.get_Desconectar_Socket();
        }

        [PaqueteAtributo("AlEv")]
        public async void get_Error_Version(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("Login", "La versión %1 de Dofus que tienes instalada no es compatible con este servidor. Para poder jugar, instala la versión %2. El cliente DOFUS se va a cerrar.");
            await cliente.get_Desconectar_Socket();
        }

        [PaqueteAtributo("AlEb")]
        public void get_Error_Baneado(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("Login", "Conexión rechazada. Tu cuenta ha sido baneada.");
            cliente.get_Desconectar_Socket();
        }

        [PaqueteAtributo("AlEd")]
        public void get_Error_Conectado(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("Login", "Esta cuenta ya está conectada a un servidor de juego. Por favor, inténtalo de nuevo.");
            cliente.get_Desconectar_Socket();
        }

        [PaqueteAtributo("AlEk")]
        public void get_Error_Baneado_Tiempo(ClienteAbstracto cliente, string paquete)
        {
            string[] informacion_ban = paquete.Substring(3).Split('|');
            int dias = int.Parse(informacion_ban[0].Substring(1)), horas = int.Parse(informacion_ban[1]), minutos = int.Parse(informacion_ban[2]);
            StringBuilder mensaje = new StringBuilder().Append("Tu cuenta estará inválida durante ");

            if (dias > 0)
                mensaje.Append(dias + " días con ");
            if (horas > 0)
                mensaje.Append(horas + " horas y ");
            if (minutos > 0)
                mensaje.Append(minutos + " minutos");

            cliente.cuenta.logger.log_Error("Login", mensaje.ToString());
            cliente.get_Desconectar_Socket();
        }
    }
}
