using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Network
{
    public class ClienteTcp : IDisposable
    {
        private Socket socket { get; set; }
        private byte[] buffer { get; set; }
        public Cuenta cuenta;
        private SemaphoreSlim semaforo;
        private bool disposed;

        public event Action<string> paquete_recibido;
        public event Action<string> paquete_enviado;
        public event Action<string> socket_informacion;

        /** ping **/
        private bool esta_esperando_paquete = false;
        private int ticks;
        private List<int> pings;

        public ClienteTcp(Cuenta _cuenta) => cuenta = _cuenta;

        public void conexion_Servidor(IPAddress ip, int puerto)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[socket.ReceiveBufferSize];
                semaforo = new SemaphoreSlim(1);
                pings = new List<int>(50);
                socket.BeginConnect(ip, puerto, new AsyncCallback(conectar_CallBack), socket);
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex.ToString());
                get_Desconectar_Socket();
            }
        }

        private void conectar_CallBack(IAsyncResult ar)
        {
            try
            {
                if (esta_Conectado())
                {
                    socket = ar.AsyncState as Socket;
                    socket.EndConnect(ar);

                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
                    socket_informacion?.Invoke("Socket conectado correctamente");
                }
                else
                {
                    get_Desconectar_Socket();
                    socket_informacion?.Invoke("Impossible enviar el socket con el host");
                }
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex.ToString());
                get_Desconectar_Socket();
            }
        }

        public void recibir_CallBack(IAsyncResult ar)
        {
            if (!esta_Conectado() || disposed)
            {
                get_Desconectar_Socket();
                return;
            }

            int bytes_leidos = socket.EndReceive(ar, out SocketError respuesta);

            if (bytes_leidos > 0 && respuesta == SocketError.Success)
            {
                string datos = Encoding.UTF8.GetString(buffer, 0, bytes_leidos);

                foreach (string paquete in datos.Replace("\x0a", string.Empty).Split('\0').Where(x => x != string.Empty))
                {
                    paquete_recibido?.Invoke(paquete);

                    if (esta_esperando_paquete)
                    {
                        pings.Add(Environment.TickCount - ticks);

                        if (pings.Count > 48)
                            pings.RemoveAt(1);

                        esta_esperando_paquete = false;
                    }

                    PaqueteRecibido.Recibir(this, paquete);
                }

                if (esta_Conectado())
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
            }
            else
                cuenta.desconectar();
        }

        public async Task enviar_Paquete_Async(string paquete, bool necesita_respuesta)
        {
            try
            {
                if (!esta_Conectado())
                    return;

                paquete += "\n\x00";
                byte[] byte_paquete = Encoding.UTF8.GetBytes(paquete);

                await semaforo.WaitAsync().ConfigureAwait(false);

                if (necesita_respuesta)
                    esta_esperando_paquete = true;

                socket.Send(byte_paquete);

                if (necesita_respuesta)
                    ticks = Environment.TickCount;

                paquete_enviado?.Invoke(paquete);
                semaforo.Release();
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex.ToString());
                get_Desconectar_Socket();
            };
        }

        public void enviar_Paquete(string paquete, bool necesita_respuesta = false) => enviar_Paquete_Async(paquete, necesita_respuesta).Wait();

        public void get_Desconectar_Socket()
        {
            if (esta_Conectado())
            {
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }

                socket_informacion?.Invoke("Socket desconectado del host");
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

        public int get_Total_Pings() => pings.Count();
        public int get_Promedio_Pings() => (int)pings.Average();
        public int get_Actual_Ping() => Environment.TickCount - ticks;

        #region Zona Dispose
        ~ClienteTcp() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }

                if (disposing)
                {
                    socket.Dispose();
                    semaforo.Dispose();
                }

                semaforo = null;
                cuenta = null;
                socket = null;
                buffer = null;
                paquete_recibido = null;
                paquete_enviado = null;
                disposed = true;
            }
        }
        #endregion
    }
}
