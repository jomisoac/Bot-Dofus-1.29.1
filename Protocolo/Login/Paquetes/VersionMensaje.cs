namespace Bot_Dofus_1._29._1.Protocolo.Login.Paquetes
{
    class VersionMensaje : Mensajes
    {
        public string get_Mensaje()
        {
            return Constantes.VERSION + "." + Constantes.SUBVERSION + "." + Constantes.SUBSUBVERSION;
        }
    }
}
