using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class ChatFrame : Frame
    {
        [PaqueteAtributo("cC+")]
        public void get_Agregar_Canal(ClienteTcp cliente, string paquete) => cliente.cuenta.juego.personaje.agregar_Canal_Personaje(paquete.Substring(3));

        [PaqueteAtributo("cC-")]
        public void get_Eliminar_Canal(ClienteTcp cliente, string paquete) => cliente.cuenta.juego.personaje.eliminar_Canal_Personaje(paquete.Substring(3));

        [PaqueteAtributo("cMK")]
        public void get_Mensajes_Chat(ClienteTcp cliente, string paquete)
        {
            string[] separador = paquete.Substring(3).Split('|');
            string canal = string.Empty;

            switch (separador[0])
            {
                case "?":
                    canal = "RECRUTEMENT";
                break;

                case ":":
                    canal = "COMMERCE";
                break;

                case "^":
                    canal = "INCARNAM";
                break;

                case "i":
                    canal = "INFORMATION";
                break;

                case "#":
                    canal = "EQUIPE";
                break;

                case "$":
                    canal = "GROUPE";
                break;

                case "%":
                    canal = "GUILDE";
                break;

                case "F":
                    cliente.cuenta.logger.log_privado("Message Reçu", separador[2] + ": " + separador[3]);
                break;

                case "T":
                    cliente.cuenta.logger.log_privado("Message Envoyé", separador[2] + ": " + separador[3]);
                break;

                default:
                    canal = "GENERAL";
                break;
            }

            if (!canal.Equals(string.Empty))
                cliente.cuenta.logger.log_normal(canal, separador[2] + ": " + separador[3]);
        }
    }
}
