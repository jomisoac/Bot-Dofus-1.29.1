using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bot_Dofus_1._29._1.Protocolo
{
    class ProtocoloSocket : Eventos
    {
        private byte[] buffer = new byte[4000];
        private Socket socket;

        public event PaqueteEntrada evento_paquete_entrada;
        public event SocketCerrado evento_socket_cerrado;
        public event Conectado evento_conectado;
        public event ConexionFallida evento_conexion_Fallida;

        //estados
        private ManualResetEvent conectar_estado = new ManualResetEvent(false);
        private ManualResetEvent enviar_estado = new ManualResetEvent(false);
        private ManualResetEvent recibir_estado = new ManualResetEvent(false);

        public ProtocoloSocket(string ip, int puerto)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(IPAddress.Parse(ip), puerto, new AsyncCallback(conectar_CallBack), socket);
                conectar_estado.WaitOne();
            }
            catch (Exception ex)
            {
                evento_Fallo_Conexion(ex);
            }
        }

        public void paquete_Recibido()
        {
            try
            {
                EstadoSocket estado = new EstadoSocket();
                estado.socket = socket;
                socket.BeginReceive(estado.buffer, 0, 256, 0, new AsyncCallback(recibir_CallBack), estado);
            }
            catch (Exception e)
            {
                evento_Fallo_Conexion(e);
            }
        }

        public void enviar_Paquete(string paquete)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(paquete + "\x00");
            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(enviar_Callback), socket);
            enviar_estado.WaitOne();
        }

        private void enviar_Callback(IAsyncResult ar)
        {
            socket = (Socket)ar.AsyncState;
            int bytes_enviados = socket.EndSend(ar);
            enviar_estado.Set();
        }

        private void conectar_CallBack(IAsyncResult ar)
        {
            try
            {
                socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);

                if (socket.Connected && socket != null)
                {
                    conectar_estado.Set();
                    evento_Conexion_Conectado();
                    paquete_Recibido();
                }
                else
                    evento_Fallo_Conexion(new Exception("No Conectado"));
            }
            catch (Exception ex)
            {
                evento_Fallo_Conexion(ex);
            }
        }
        
        private void recibir_CallBack(IAsyncResult ar)
        {
            EstadoSocket state = (EstadoSocket)ar.AsyncState;
            socket = state.socket;

            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                evento_Nuevo_Paquete(state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead)).ToString());
                paquete_Recibido();
            }
            else
            {
                recibir_estado.Set();
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

        public void cerrar_Socket()
        {
            socket.Close();
        }

        private void evento_Nuevo_Paquete(string paquete)
        {
            evento_paquete_entrada?.Invoke(paquete);
        }

        private void evento_Socket_Cerrado()
        {
            evento_socket_cerrado?.Invoke();
        }

        private void evento_Fallo_Conexion(Exception ex)
        {
            evento_conexion_Fallida?.Invoke(ex);
        }

        private void evento_Conexion_Conectado()
        {
            evento_conectado?.Invoke();
        }
    }
}
