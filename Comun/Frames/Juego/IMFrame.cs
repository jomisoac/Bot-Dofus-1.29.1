using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using System.Threading.Tasks;

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
        public void get_Mensaje_Bienvenida_Dofus(TcpClient cliente, string paquete) => cliente.account.Logger.LogError("DOFUS", "Bienvenue à DOFUS, le Monde des Douze ! Attention Il est interdit de communiquer le nom d'utilisateur et le mot de passe de votre compte.");

        [PaqueteAtributo("Im039")]
        public void get_Pelea_Espectador_Desactivado(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("COMBAT", "Le mode Spectator est désactivé.");

        [PaqueteAtributo("Im040")]
        public void get_Pelea_Espectador_Activado(TcpClient cliente, string paquete)
        {
            cliente.account.Logger.LogInfo("COMBAT", "Le mode Spectator est activé.");
        }

        [PaqueteAtributo("Im0152")]
        public void get_Mensaje_Ultima_Conexion_IP(TcpClient cliente, string paquete)
        {
            string mensaje = paquete.Substring(3).Split(';')[1];
            cliente.account.Logger.LogInfo("DOFUS", "Dernière connexion à votre compte effectuée le " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " à " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + " adresse IP " + mensaje.Split('~')[5]);
        }

        [PaqueteAtributo("Im0153")]
        public async void get_Mensaje_Nueva_Conexion_IP(TcpClient cliente, string paquete)
        {
            cliente.account.Logger.LogInfo("DOFUS", "Votre adresse IP actuelle est " + paquete.Substring(3).Split(';')[1]);
            /* invitation groupe par leader */
            if (cliente.account.isGroupLeader == true && cliente.account.hasGroup ==true)
            {

                foreach (var membre in cliente.account.group.members)
                {

                    await Task.Delay(780);

                    if ((membre.AccountState == AccountStates.CONNECTED || membre.AccountState == AccountStates.CONNECTED_INACTIVE))
                    {
                        cliente.account.Logger.LogInfo("GROUPE", "J'invite dans mon groupe :" + membre.game.character.nombre);
                        cliente.SendPacket("PI" + membre.game.character.nombre, false);

                        await Task.Delay(1080);
                    }
                }
            }
            else if (cliente.account.hasGroup == true && (cliente.account.group.lider.AccountState == AccountStates.CONNECTED || cliente.account.group.lider.AccountState == AccountStates.CONNECTED_INACTIVE))
            {
                await Task.Delay(580);
                Account leader = cliente.account.group.lider;
                leader.connexion.SendPacket("PI" + cliente.account.game.character.nombre, false);
                leader.Logger.LogInfo("GROUPE", "J'invite dans mon groupe :" + cliente.account.game.character.nombre);
                await Task.Delay(1080);
            }

        }

        [PaqueteAtributo("Im020")]
        public void get_Mensaje_Abrir_Cofre_Perder_Kamas(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("DOFUS", "Vous avez dû donner " + paquete.Split(';')[1] + " kamas pour accéder à ce coffre.");

        [PaqueteAtributo("Im025")]
        public void get_Mensaje_Mascota_Feliz(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("DOFUS", "Votre animal est si heureux de vous revoir !");

        [PaqueteAtributo("Im0157")]
        public void get_Mensaje_Error_Chat_Difusion(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("DOFUS", "Ce canal est seulement disponible aux abonnés de niveau " + paquete.Split(';')[1]);

        [PaqueteAtributo("Im037")]
        public void get_Mensaje_Modo_Away_Dofus(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("DOFUS", "Désormais, tu seras considéré comme absent.");

        [PaqueteAtributo("Im112")]
        public void get_Mensaje_Pods_Llenos(TcpClient cliente, string paquete) => cliente.account.Logger.LogError("DOFUS", "Tu es trop chargé. Jetez quelques objets pour pouvoir bouger.");
    }
}
