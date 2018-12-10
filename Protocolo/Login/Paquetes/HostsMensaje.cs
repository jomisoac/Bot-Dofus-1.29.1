namespace Bot_Dofus_1._29._1.Protocolo.Login.Paquetes
{
    public class HostsMensaje : Mensajes
    {
        public bool es_accesible = true;
        public byte estado = 0;

        public HostsMensaje(string paquete, int serverID)
        {
            string[] _loc5_ = paquete.Split('|');
            int _loc6_ = 0;
            while (_loc6_ < _loc5_.Length)
            {
                string[] _loc7_ = _loc5_[_loc6_].ToString().Split(';');
                int id = int.Parse(_loc7_[0]);
                estado = byte.Parse(_loc7_[1]);
                byte poblacion = byte.Parse(_loc7_[2]);
                bool registro = _loc7_[3] == "1";

                if (id == serverID && estado != 1)
                {
                    es_accesible = false;
                }
                _loc6_++;
            }
        }

        public string get_Mensaje()
        {
            return "Ax";
        }

        public enum EstadosServidor
        {
            APAGADO,
            CONECTADO,
            GUARDANDO
        }
    }
}
