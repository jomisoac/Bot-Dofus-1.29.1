using System;
using System.Net.Sockets;
using static Bot_Dofus_1._29._1.Protocolo.Eventos;

namespace Bot_Dofus_1._29._1.Protocolo
{
    class ProtocoloSocket
    {
        private object bloqueo = new object();
        private byte[] buffer = new byte[3004];
        private Socket socket;

        public event PaqueteEntrada paquete_entrada_evento;
        public event SocketCerrado socket_cerrado_evento;
        public event Conectado conectado_evento;
        public event ConexionFallida conexion_Fallida_evento;

        public ProtocoloSocket(Socket conexion, int tamano_buffer)
        {
            try
            {
                buffer = new byte[tamano_buffer];
                socket = conexion;
                //ThreadReceived();
            }
            catch (Exception){}
        }
    }
}
