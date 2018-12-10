using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;

namespace Bot_Dofus_1._29._1.Protocolo.Login.Paquetes
{
    class LoginMensaje : Mensajes
    {
        private string usuario, password;

        public LoginMensaje(String _usuario, String _password, String key_bienvenida)
        {
            usuario = _usuario;
            password = Hash.encriptar_Password(_password, key_bienvenida);
        }

        public string get_Mensaje()
        {
            return usuario + '\n' + password;
        }
    }
}
