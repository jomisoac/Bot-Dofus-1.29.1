using Bot_Dofus_1._29._1.Otros;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Network
{
    public abstract class ClienteAbstracto
    {
        protected Socket socket { get; private set; }
        protected byte[] buffer { get; set; }
        public Cuenta cuenta;
        private bool disposed;

        public event Action<string> paquete_recibido;
        public event Action<string> paquete_enviado;
        public event Action<object> socket_informacion;

        public ClienteAbstracto(string ip, int puerto)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[6000];
                socket.BeginConnect(IPAddress.Parse(ip), puerto, new AsyncCallback(conectar_CallBack), socket);
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex);
                get_Desconectar_Socket();
            }
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
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
                }
                else
                {
                    get_Desconectar_Socket();
                    socket_informacion?.Invoke("Impossible enviar el socket con el host");
                }
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex);
                get_Desconectar_Socket();
            }
        }

        public virtual void recibir_CallBack(IAsyncResult ar) { }

        public void enviar_Paquete(string paquete)
        {
            try
            {
                if (esta_Conectado())
                {
                    socket.Send(Encoding.UTF8.GetBytes(string.Format("{0}\n\x00", paquete)));
                    paquete_enviado?.Invoke(paquete);
                }
                else
                {
                    socket_informacion?.Invoke("Impossible enviar el socket, no conectado con el host");
                    get_Desconectar_Socket();
                }
            }
            catch (Exception e)
            {
                socket_informacion?.Invoke(e);
                get_Desconectar_Socket();
            };
        }

        public void get_Desconectar_Socket()
        {
            if (esta_Conectado())
            {
                disposed = true;
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket_informacion?.Invoke("Socket desconectado del host");
                Dispose();
            }
        }

        public bool esta_Conectado()
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

        protected void get_Evento_Recibido(string mensaje) => paquete_recibido?.Invoke(mensaje);
        protected void get_Evento_Enviado(string mensaje) => paquete_enviado?.Invoke(mensaje);
        protected void get_Evento_Informacion(string mensaje) => socket_informacion?.Invoke(mensaje);

        #region Zona Dispose
        ~ClienteAbstracto() => Dispose(false);

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
                    socket.Dispose();

                buffer = null;
                paquete_recibido = null;
                paquete_enviado = null;
                socket_informacion = null;
                disposed = true;
            }
        }
        #endregion
    }
}
