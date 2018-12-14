using System;

namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    internal class TiquetRespuesta : Mensajes
    {
        private readonly short tiquet;
        public static char[] HEX_CHARS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public TiquetRespuesta(string sExtraData)
        {
            tiquet = Convert.ToInt16(sExtraData.Substring(0, 1));
        }

        public string get_Mensaje()
        {
            return "Ak" + HEX_CHARS[tiquet];
        }
    }
}
