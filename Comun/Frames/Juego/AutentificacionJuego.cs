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
    class AutentificacionJuego : Frame
    {
        [PaqueteAtributo("M030")]
        public void get_Error_Streaming(TcpClient cliente, string paquete)
        {
            cliente.account.Logger.LogError("Login", "Connexion rejetée. Vous n'avez pas pu vous authentifier pour ce serveur car votre connexion a expiré. Assurez-vous de couper les téléchargements, la musique ou les vidéos en continu pour améliorer la qualité et la vitesse de votre connexion.");
            cliente.account.Disconnect();
        }

        [PaqueteAtributo("M031")]
        public void get_Error_Red(TcpClient cliente, string paquete)
        {
            cliente.account.Logger.LogError("Login", "Connexion rejetée. Le serveur de jeu n'a pas reçu les informations d'authentification nécessaires après votre identification. Veuillez réessayer et, si le problème persiste, contactez votre administrateur réseau ou votre serveur d'accès Internet. C'est un problème de redirection dû à une mauvaise configuration DNS.");
            cliente.account.Disconnect();
        }

        [PaqueteAtributo("M032")]
        public void get_Error_Flood_Conexion(TcpClient cliente, string paquete)
        {
            cliente.account.Logger.LogError("Login", "Pour éviter de déranger les autres joueurs, attendez %1 secondes avant de vous reconnecter.");
            cliente.account.Disconnect();
        }
    }
}
