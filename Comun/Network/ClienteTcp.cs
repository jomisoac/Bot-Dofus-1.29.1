using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using System;
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
        protected Socket socket { get; private set; }
        protected byte[] buffer { get; set; }
        public Cuenta cuenta;
        private EstadoSocket fase_socket;
        private SemaphoreSlim semaforo;
        protected bool disposed;

        public event Action<string> paquete_recibido;
        public event Action<string> paquete_enviado;
        public event Action<string> socket_informacion;
        public event Action<ClienteTcp> evento_fase_socket;

        public ClienteTcp(Cuenta _cuenta) => cuenta = _cuenta;

        public void conexion_Servidor(IPAddress ip, int puerto)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                buffer = new byte[socket.ReceiveBufferSize];
                Estado_Socket = EstadoSocket.NINGUNO;
                semaforo = new SemaphoreSlim(1);
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
                    Program.paquete_recibido.Recibir(this, paquete);
                    paquete_recibido?.Invoke(paquete);
                }

                if (esta_Conectado())
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recibir_CallBack), socket);
                else
                    get_Desconectar_Socket();
            }
            else
                get_Desconectar_Socket();

        }

        public async Task enviar_Paquete_Async(string paquete)
        {
            try
            {
                if (!esta_Conectado())
                    return;

                paquete += "\n\x00";
                byte[] byte_paquete = Encoding.UTF8.GetBytes(paquete);

                await semaforo.WaitAsync().ConfigureAwait(false);

                socket.Send(byte_paquete);
                paquete_enviado?.Invoke(paquete);

                semaforo.Release();
            }
            catch (Exception ex)
            {
                socket_informacion?.Invoke(ex.ToString());
                get_Desconectar_Socket();
            };
        }

        public void enviar_Paquete(string paquete) => enviar_Paquete_Async(paquete).Wait();

        public void get_Desconectar_Socket()
        {
            if (esta_Conectado())
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                socket_informacion?.Invoke("Socket desconectado del host");

                if (Estado_Socket != EstadoSocket.CAMBIANDO_A_JUEGO)
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

        public EstadoSocket Estado_Socket
        {
            get => fase_socket;
            internal set
            {
                EstadoSocket antiguo_valor = fase_socket;
                fase_socket = value;

                if (antiguo_valor != fase_socket)
                    evento_fase_socket?.Invoke(this);
            }
        }

        #region Zona Dispose
        ~ClienteTcp() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if(socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
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
                socket_informacion = null;
                fase_socket = EstadoSocket.DESCONECTADO;
                disposed = true;
            }
        }
        #endregion
    }
}
