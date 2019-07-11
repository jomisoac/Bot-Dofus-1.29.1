using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class IMFrame : Frame
    {
        [PaqueteAtributo("Im189")]
        public void get_Mensaje_Bienvenida_Dofus(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_Error("DOFUS", "¡Bienvenido(a) a DOFUS, el Mundo de los Doce! Atención: Está prohibido comunicar tu usuario de cuenta y tu contraseña.");

        [PaqueteAtributo("Im039")]
        public void get_Pelea_Espectador_Desactivado(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("COMBATE", "El modo Espectador está desactivado.");

        [PaqueteAtributo("Im040")]
        public void get_Pelea_Espectador_Activado(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("COMBATE", "El modo Espectador está activado.");

        [PaqueteAtributo("Im0152")]
        public void get_Mensaje_Ultima_Conexion_IP(ClienteTcp cliente, string paquete)
        {
            string mensaje = paquete.Substring(3).Split(';')[1];
            cliente.cuenta.logger.log_informacion("DOFUS", "Última conexión a tu cuenta realizada el " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " a las " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + " mediante la dirección IP " + mensaje.Split('~')[5]);
        }

        [PaqueteAtributo("Im0153")]
        public void get_Mensaje_Nueva_Conexion_IP(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Tu dirección IP actual es " + paquete.Substring(3).Split(';')[1]);

        [PaqueteAtributo("Im020")]
        public void get_Mensaje_Abrir_Cofre_Perder_Kamas(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Has tenido que dar " + paquete.Split(';')[1] + " kamas para poder acceder a este cofre.");

        [PaqueteAtributo("Im025")]
        public void get_Mensaje_Mascota_Feliz(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "¡Tu mascota está muy contenta de volver a verte!");

        [PaqueteAtributo("Im0157")]
        public void get_Mensaje_Error_Chat_Difusion(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Este canal no tiene la difusión accesible más que a los abonados de nivel " + paquete.Split(';')[1]);

        [PaqueteAtributo("Im037")]
        public void get_Mensaje_Modo_Away_Dofus(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Desde ahora serás considerado como ausente.");

        [PaqueteAtributo("Im112")]
        public void get_Mensaje_Pods_Llenos(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_Error("DOFUS", "Estás demasiado cargado. Tira algunos objetos para poder moverte.");
    }
}
