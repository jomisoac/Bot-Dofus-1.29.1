using System;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Protocolo.Login.Paquetes
{
    class ListaServidoresMensaje
    {
        public int servidor_id { get; set; }
        TimeSpan tiempo_abono { get; set; }
        public Dictionary<int, int> servidores { get; set; }

        public ListaServidoresMensaje(string paquete, int _servidor_id)
        {
            servidores = new Dictionary<int, int>();
            servidor_id = _servidor_id;

            string[] loc5 = paquete.Split('|');

            for (int i = 1; i < loc5.Length; ++i)
            {
                var _loc10_ = loc5[i].Split(',');
                int _sID = int.Parse(_loc10_[0]);
                int _chara = int.Parse(_loc10_[1]);
                if (!servidores.ContainsKey(_sID))
                {
                    servidores.Add(_sID, _chara);
                }
            }
        }

        public string get_Mensaje()
        {
            if (servidores.ContainsKey(servidor_id))
                return "AX" + servidor_id;
            else
                return null;
        }
    }
}
