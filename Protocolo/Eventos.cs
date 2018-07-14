using System;

namespace Bot_Dofus_1._29._1.Protocolo
{
    class Eventos
    {
        public delegate void Escuchando();
        public delegate void EscuchaFallida(Exception ex);
        public delegate void SocketAceptado(ProtocoloSocket socket);
        public delegate void PaqueteEntrada(string paquete);
        public delegate void Conectado();
        public delegate void ConexionFallida(Exception ex);
        public delegate void SocketCerrado();
    }
}
