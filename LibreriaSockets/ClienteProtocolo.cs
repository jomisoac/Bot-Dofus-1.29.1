using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bot_Dofus_1._29._1.LibreriaSockets
{
    public abstract class ClienteProtocolo : ProtocoloInterfaz
    {
        protected Socket socket;
        private byte[] buffer = new byte[5000];
        private readonly object bloqueo = new object();

        public event Action<string> evento_paquete_recibido;
        public event Action<string> evento_paquete_enviado;
        public event Action<object> evento_socket_informacion;

        public ClienteProtocolo(string ip, int puerto)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(o =>
            {
                try
                {
                   
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(IPAddress.Parse(ip), puerto, new AsyncCallback(conectar_CallBack), socket);
                }
                catch (Exception ex)
                {
                    evento_socket_informacion?.Invoke(ex);
                }
            }));
        }

        private void conectar_CallBack(IAsyncResult ar)
        {
            try
            {
                socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);

                if (socket.Connected || socket != null)
                {
                    evento_socket_informacion?.Invoke("Socket conectado correctamente");
                    paquete_Recibido();
                }
                else
                {
                    evento_socket_informacion?.Invoke("Impossible conectar el socket con el host");
                }
            }
            catch (Exception ex)
            {
                evento_socket_informacion?.Invoke(ex);
            }
        }

        private void paquete_Recibido()
        {
            if (socket.Connected && socket != null)
            {
                try
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
                }
                catch (Exception e)
                {
                    evento_socket_informacion?.Invoke(e.Message);
                }
            }
        }

        private void recibir_CallBack(IAsyncResult ar)
        {
            lock (bloqueo)
            {
                try
                {
                    int length = socket.EndReceive(ar);
                    if (length > 0)
                    {
                        foreach (string paquete in Encoding.UTF8.GetString(buffer).Replace("\x0a", "").Split('\x00').Where(x => x != ""))
                        {
                            evento_paquete_recibido?.Invoke(paquete);
                        }
                        buffer = new byte[5000];
                        paquete_Recibido();
                    }
                }
                catch (Exception e)
                {
                    evento_socket_informacion?.Invoke(e);
                }
            }
        }

        public void enviar_Paquete(string paquete)
        {
            lock (bloqueo)
            {
                if (socket.Connected)
                {
                    socket.Send(Encoding.UTF8.GetBytes(string.Format(paquete + "\n\0")));
                    evento_paquete_enviado?.Invoke(paquete);
                }
                else
                {
                    evento_socket_informacion?.Invoke("Impossible enviar el socket, no conectado con el host");
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

        protected void cerrar_Socket()
        {
            lock (bloqueo)
            {
                try
                {
                    socket.Close();
                    evento_socket_informacion?.Invoke("Socket desconectado del host");
                }
                catch (Exception e)
                {
                    evento_socket_informacion?.Invoke(e.Message);
                }
            }
        }

        public void Dispose()
        {
            socket = null;
            buffer = null;
        }
    }
}
