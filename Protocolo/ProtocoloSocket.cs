using System;
using System.Net;
using System.Net.Sockets;

namespace Bot_Dofus_1._29._1.Protocolo
{
    class ProtocoloSocket : Eventos
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
            buffer = new byte[tamano_buffer];
            socket = conexion;
            paquete_Recibido();
        }

        public void Conectar(string ip, int puerto)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(IPAddress.Parse(ip), puerto, new AsyncCallback(conectar_CallBack), socket);
            }
            catch (Exception ex)
            {
                evento_Fallo_Conexion(ex);
            }
        }

        public void paquete_Recibido()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
        }

        public void enviar_Paquete(byte[] paquete)
        {
            lock (bloqueo)
            {
                socket.Send(paquete);
            }
        }

        public void cerrar_Socket()
        {
            lock (bloqueo)
            {
                socket.Close();
            }
        }

        private void conectar_CallBack(IAsyncResult ar)
        {
            try
            {
                if (socket.Connected)
                {
                    evento_Conexion_Conectado();
                    paquete_Recibido();
                }
                else
                    evento_Fallo_Conexion(new Exception("Not Connect"));
            }
            catch (Exception ex)
            {
                evento_Fallo_Conexion(ex);
            }
        }

        private void recibir_CallBack(IAsyncResult ar)
        {
            lock (bloqueo)
            {
                int longitud = socket.EndReceive(ar);
                if (longitud > 0)
                {
                    byte[] data = new byte[longitud];
                    for (int i = 0; i <= longitud - 1; i++)
                        data[i] = buffer[i];
                    evento_Nuevo_Paquete(data);
                    paquete_entrada_evento?.Invoke(data);
                    buffer = new byte[5000];
                    paquete_Recibido();
                }
                else
                {
                    evento_Socket_Cerrado();
                }
            }
        }

        public string IP
        {
            get
            {
                try
                {
                    return socket.RemoteEndPoint.ToString();
                }
                catch (Exception)
                {
                    return "null";
                }
            }
        }

        private void evento_Nuevo_Paquete(byte[] paquete)
        {
            paquete_entrada_evento?.Invoke(paquete);
        }

        private void evento_Socket_Cerrado()
        {
            socket_cerrado_evento?.Invoke();
        }

        private void evento_Fallo_Conexion(Exception ex)
        {
            conexion_Fallida_evento?.Invoke(ex);
        }

        private void evento_Conexion_Conectado()
        {
            conectado_evento?.Invoke();
        }
    }
}
