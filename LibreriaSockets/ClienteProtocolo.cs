using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bot_Dofus_1._29._1.LibreriaSockets
{
    public abstract class ClienteProtocolo : IDisposable
    {
        protected Socket socket;
        private byte[] buffer = new byte[5000];
        private readonly object bloqueo = new object();

        public event Action<string> evento_paquete_recibido;
        public event Action<string> evento_paquete_enviado;
        public event Action<object> evento_socket_informacion;
        public event Action<object> evento_socket_desconectado;

        public ClienteProtocolo(string ip, int puerto)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(o =>
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(IPAddress.Parse(ip), puerto, conectar_CallBack, socket);
                }
                catch (Exception ex)
                {
                    evento_socket_informacion?.Invoke(ex);
                    cerrar_Socket();
                }
            }));
        }

        private void conectar_CallBack(IAsyncResult ar)
        {
            try
            {
                socket = ar.AsyncState as Socket;
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
                cerrar_Socket();
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
                    cerrar_Socket();
                }
            }
        }

        private void recibir_CallBack(IAsyncResult ar)
        {
            lock (bloqueo)
            {
                try
                {
                    int bytes_leidos = socket.EndReceive(ar);
                    if (bytes_leidos != 0)
                    {
                        foreach (string paquete in Encoding.UTF8.GetString(buffer).Replace("\x0a", "").Split('\x00').Where(x => x != ""))
                        {
                            evento_paquete_recibido?.Invoke(paquete);
                        }
                        buffer = new byte[5000];
                        paquete_Recibido();
                    }
                }
                catch (ObjectDisposedException e)
                {
                    cerrar_Socket();
                }
                catch (Exception e)
                {
                    evento_socket_informacion?.Invoke(e);
                    cerrar_Socket();
                }
            }
        }

        public void enviar_Paquete(string paquete)
        {
            lock (bloqueo)
            {
                try
                {
                    if (socket.Connected)
                    {
                        byte[] mensaje_bytes = Encoding.UTF8.GetBytes(string.Format(paquete + "\n\0"));
                        socket.BeginSend(mensaje_bytes, 0, mensaje_bytes.Length, SocketFlags.None, new AsyncCallback(enviar_CallBack), socket);
                        evento_paquete_enviado?.Invoke(paquete);
                    }
                    else
                    {
                        evento_socket_informacion?.Invoke("Impossible enviar el socket, no conectado con el host");
                        cerrar_Socket();
                    }
                }
                catch (Exception e)
                {
                    evento_socket_informacion?.Invoke(e);
                    cerrar_Socket();
                };
            }
        }

        private void enviar_CallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = ar.AsyncState as Socket;
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                evento_socket_informacion?.Invoke(e);
                cerrar_Socket();
            }
        }

        public void cerrar_Socket()
        {
            lock (bloqueo)
            {
                try
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        evento_socket_desconectado?.Invoke("Socket desconectado del host");
                    }
                }
                catch (Exception e)
                {
                    evento_socket_desconectado?.Invoke(e.Message);
                }
            }
        }

        ~ClienteProtocolo()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                    buffer = null;
                }
                catch { }
            }
        }
    }
}
