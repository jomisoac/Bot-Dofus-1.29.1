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
        public void get_Mensaje_Bienvenida_Dofus(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_Error("DOFUS", "Bienvenue à DOFUS, le Monde des Douze ! Attention Il est interdit de communiquer le nom d'utilisateur et le mot de passe de votre compte.");

        [PaqueteAtributo("Im039")]
        public void get_Pelea_Espectador_Desactivado(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("COMBAT", "Le mode Spectator est désactivé.");

        [PaqueteAtributo("Im040")]
        public void get_Pelea_Espectador_Activado(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("COMBAT", "Le mode Spectator est activé.");

        [PaqueteAtributo("Im0152")]
        public void get_Mensaje_Ultima_Conexion_IP(ClienteTcp cliente, string paquete)
        {
            string mensaje = paquete.Substring(3).Split(';')[1];
            cliente.cuenta.logger.log_informacion("DOFUS", "Dernière connexion à votre compte effectuée le " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " à " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + " adresse IP " + mensaje.Split('~')[5]);
        }

        [PaqueteAtributo("Im0153")]
        public void get_Mensaje_Nueva_Conexion_IP(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Votre adresse IP actuelle est " + paquete.Substring(3).Split(';')[1]);

        [PaqueteAtributo("Im020")]
        public void get_Mensaje_Abrir_Cofre_Perder_Kamas(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Vous avez dû donner " + paquete.Split(';')[1] + " kamas pour accéder à ce coffre.");

        [PaqueteAtributo("Im025")]
        public void get_Mensaje_Mascota_Feliz(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Votre animal est si heureux de vous revoir !");

        [PaqueteAtributo("Im0157")]
        public void get_Mensaje_Error_Chat_Difusion(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Ce canal est seulement disponible aux abonnés de niveau " + paquete.Split(';')[1]);

        [PaqueteAtributo("Im037")]
        public void get_Mensaje_Modo_Away_Dofus(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", "Désormais, tu seras considéré comme absent.");

        [PaqueteAtributo("Im112")]
        public void get_Mensaje_Pods_Llenos(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_Error("DOFUS", "Tu es trop chargé. Jetez quelques objets pour pouvoir bouger.");
    }
}
