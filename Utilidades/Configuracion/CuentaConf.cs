using System.IO;

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    public class CuentaConf
    {
        private string nombre_cuenta, password, servidor, nombre_personaje;

        public CuentaConf(string _nombre_cuenta, string _password, string _servidor, string _nombre_personaje)
        {
            nombre_cuenta = _nombre_cuenta;
            password = _password;
            servidor = _servidor;
            nombre_personaje = _nombre_personaje;
        }

        public void guardar_Cuenta(BinaryWriter bw)
        {
            bw.Write(nombre_cuenta);
            bw.Write(password);
            bw.Write(servidor);
            bw.Write(nombre_personaje);
        }

        public static CuentaConf cargar_Una_Cuenta(BinaryReader br)
        {
            try
            {
                return new CuentaConf(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString());
            }
            catch
            {
                return null;
            }
        }

        public string get_Nombre_Cuenta()
        {
            return nombre_cuenta;
        }

        public string get_Password()
        {
            return password;
        }

        public string get_Servidor()
        {
            return servidor;
        }

        public int get_Servidor_Id()
        {
            return servidor.Equals("Eratz") ? 601 : 602;
        }

        public string get_nombre_personaje()
        {
            return nombre_personaje;
        }
    }
}
