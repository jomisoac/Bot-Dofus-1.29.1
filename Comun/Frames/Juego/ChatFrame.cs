using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class ChatFrame : Frame
    {
        [PaqueteAtributo("cC+")]
        public void get_Agregar_Canal(ClienteAbstracto cliente, string paquete) => cliente.cuenta.personaje.agregar_Canal_Personaje(paquete.Substring(3));

        [PaqueteAtributo("cC-")]
        public void get_Eliminar_Canal(ClienteAbstracto cliente, string paquete) => cliente.cuenta.personaje.eliminar_Canal_Personaje(paquete.Substring(3));

        [PaqueteAtributo("cMK")]
        public void get_Mensajes_Chat(ClienteAbstracto cliente, string paquete)
        {
            string[] separador = paquete.Substring(3).Split('|');
            string canal = string.Empty;

            switch (separador[0])
            {
                case "?":
                    canal = "RECLUTAMIENTO";
                break;

                case ":":
                    canal = "COMERCIO";
                break;

                case "^":
                    canal = "INCARNAM";
                break;

                case "i":
                    canal = "INFORMACIÓN";
                break;

                case "#":
                    canal = "EQUIPO";
                break;

                case "$":
                    canal = "GRUPO";
                break;

                case "F":
                    cliente.cuenta.logger.log_privado("PRIVADO", separador[2] + ": " + separador[3]);
                break;

                default:
                    canal = "General";
                break;
            }

            if (!canal.Equals(string.Empty))
                cliente.cuenta.logger.log_normal(canal, separador[2] + ": " + separador[3]);
        }
    }
}
