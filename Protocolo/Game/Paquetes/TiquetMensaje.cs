namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    public class TiquetMensaje : Mensajes
    {
        private string tiquet_juego;

        public TiquetMensaje(string _tiquet_juego)
        {
            tiquet_juego = _tiquet_juego;
        }

        public string get_Mensaje()
        {
            return "AT" + tiquet_juego;
        }
    }
}
