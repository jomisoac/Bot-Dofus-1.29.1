using System.IO;

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    public class CuentaConfiguracion
    {
        private string nombre_cuenta, password, servidor, nombre_personaje;

        public CuentaConfiguracion(string _nombre_cuenta, string _password, string _servidor, string _nombre_personaje)
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

        public static CuentaConfiguracion cargar_una_Cuenta(BinaryReader br)
        {
            try
            {
                return new CuentaConfiguracion(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString());
            }
            catch
            {
                return null;
            }
        }

        public string get_Nombre_cuenta()
        {
            return nombre_cuenta;
        }

        public string get_password()
        {
            return password;
        }

        public string get_servidor()
        {
            return servidor;
        }

        public string get_nombre_personaje()
        {
            return nombre_personaje;
        }
    }
}
