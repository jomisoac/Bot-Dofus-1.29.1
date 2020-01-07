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
        public void WrongCredentialsError(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.Logger.LogError("LOGIN", "Connexion rejetée. Nom de compte ou mot de passe incorrect.");
            prmClient.account.Disconnect();
        }

        [PaqueteAtributo("AlEa")]
        public void AlreadyConnectedError(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.Logger.LogError("LOGIN", "Déjà connecté. Essayez encore une fois.");
            prmClient.account.Disconnect();
        }

        [PaqueteAtributo("AlEv")]
        public void WrongVersionError(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.Logger.LogError("LOGIN", "La version %1 de Dofus que vous avez installée n'est pas compatible avec ce serveur. Pour jouer, installez la version %2. Le client DOFUS sera fermé.");
            prmClient.account.Disconnect();
        }

        [PaqueteAtributo("AlEb")]
        public void AccountBannedError(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.Logger.LogError("LOGIN", "Connexion rejetée. Votre compte a été banni.");
            prmClient.account.Disconnect();
        }

        [PaqueteAtributo("AlEd")]
        public void AlreadyConnectingError(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.Logger.LogError("LOGIN", "Ce compte est déjà connecté à un serveur de jeu. Veuillez réessayer.");
            prmClient.account.Disconnect();
        }

        [PaqueteAtributo("AlEk")]
        public void AccountTempBannedError(TcpClient prmClient, string prmPacket)
        {
            string[] ban_Informations = prmPacket.Substring(3).Split('|');
            int days = int.Parse(ban_Informations[0].Substring(1)), hours = int.Parse(ban_Informations[1]), minutes = int.Parse(ban_Informations[2]);
            StringBuilder banInformationsMessage = new StringBuilder().Append("Votre compte sera invalide pendant ");

            if (days > 0)
                banInformationsMessage.Append(days + " jour(s)");
            if (hours > 0)
                banInformationsMessage.Append(hours + " heures");
            if (minutes > 0)
                banInformationsMessage.Append(minutes + " minutes");

            prmClient.account.Logger.LogError("LOGIN", banInformationsMessage.ToString());
            prmClient.account.Disconnect();
        }
    }
}
