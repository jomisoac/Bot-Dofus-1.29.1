using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.LibreriaSockets
{
    public abstract class ClienteProtocolo : IDisposable
    {
        protected Socket socket;
        private byte[] buffer;
        private readonly object bloqueo = new object();
        private bool disposed;

        public event Action<string> paquete_recibido;
        public event Action<string> paquete_enviado;
        public event Action<object> socket_informacion;
        public event Action<object> socket_desconectado;

        public ClienteProtocolo(string ip, int puerto)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(o =>
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    buffer = new byte[5000];
                    socket.BeginConnect(IPAddress.Parse(ip), puerto, conectar_CallBack, socket);
                }
                catch (Exception ex)
                {
                    socket_informacion?.Invoke(ex);
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

                if (esta_Conectado())
                {
                    socket_informacion?.Invoke("Socket conectado correctamente");
                    paquete_Recibido();
                }
                else
                {
                    cerrar_Socket();
                    socket_informacion?.Invoke("Impossible conectar el socket con el host");
                }
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex);
                cerrar_Socket();
            }
        }

        private void paquete_Recibido(int buffersize = -1)
        {
            try
            {
                if (esta_Conectado())
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
                }
                else
                {
                    cerrar_Socket();
                    return;
                }
            }
            catch (Exception e)
            {
                socket_informacion?.Invoke(e.Message);
                cerrar_Socket();
            }
        }

        private void recibir_CallBack(IAsyncResult ar)
        {
            lock (bloqueo)
            {
                try
                {
                    if (esta_Conectado())
                    {
                        int bytes_leidos = socket.EndReceive(ar, out SocketError err);
                        if (bytes_leidos > 0 || err == SocketError.Success)
                        {
                            foreach (string paquete in Encoding.UTF8.GetString(buffer).Replace("\x0a", string.Empty).Split('\x00').Where(x => x != string.Empty))
                            {
                                paquete_recibido?.Invoke(paquete);
                            }
                            buffer = new byte[5000];
                            paquete_Recibido();
                        }
                        else
                        {
                            cerrar_Socket();
                            return;
                        }
                    }
                    else
                    {
                        cerrar_Socket();
                        return;
                    }
                }
                catch (Exception e)
                {
                    socket_informacion?.Invoke(e);
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
                    if (esta_Conectado())
                    {
                        byte[] mensaje_bytes = Encoding.UTF8.GetBytes(string.Format(paquete + "\n\0"));
                        socket.BeginSend(mensaje_bytes, 0, mensaje_bytes.Length, SocketFlags.None, new AsyncCallback(enviar_CallBack), socket);
                        paquete_enviado?.Invoke(paquete);
                    }
                    else
                    {
                        socket_informacion?.Invoke("Impossible enviar el socket, no conectado con el host");
                        cerrar_Socket();
                    }
                }
                catch (Exception e)
                {
                    socket_informacion?.Invoke(e);
                    cerrar_Socket();
                };
            }
        }

        private void enviar_CallBack(IAsyncResult ar)
        {
            try
            {
                if (esta_Conectado())
                {
                    Socket handler = ar.AsyncState as Socket;
                    int enviado = handler.EndSend(ar);
                    if (enviado < 0)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        return;
                    }
                }
                else
                {
                    cerrar_Socket();
                    return;
                }
            }
            catch (Exception e)
            {
                socket_informacion?.Invoke(e);
                cerrar_Socket();
            }
        }

        public void cerrar_Socket()
        {
            lock (bloqueo)
            {
                if (esta_Conectado())
                {
                    disposed = true;
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket_desconectado?.Invoke("Socket desconectado del host");
                    Dispose();
                }
            }
        }

        private bool esta_Conectado()
        {
            try
            {
                return !(disposed || socket == null || !socket.Connected && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                if (disposing)
                {
                    socket.Dispose();
                }
                buffer = null;
                paquete_recibido = null;
                paquete_enviado = null;
                socket_informacion = null;
                socket_desconectado = null;
                disposed = true;
            }
        }
    }
}
