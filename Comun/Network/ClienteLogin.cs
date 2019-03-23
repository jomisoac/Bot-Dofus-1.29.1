using Bot_Dofus_1._29._1.Otros;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Bot_Dofus_1._29._1.Comun.Network
{
    public class ClienteLogin : ClienteAbstracto
    {
        public ClienteLogin(string ip, int puerto, Cuenta _cuenta) : base(ip, puerto) => cuenta = _cuenta;

        public override void recibir_CallBack(IAsyncResult ar)
        {
            Socket handler = ar.AsyncState as Socket;

            if (esta_Conectado())
            {
                int bytes_leidos = socket.EndReceive(ar, out SocketError respuesta);

                if (bytes_leidos > 0 && respuesta == SocketError.Success)
                {
                    string datos = Encoding.UTF8.GetString(buffer, 0, bytes_leidos);

                    foreach (string paquete in datos.Replace("\x0a", string.Empty).Split('\x00').Where(x => x != string.Empty))
                    {
                        Program.paquete_recibido.Recibir(this, paquete);
                        get_Evento_Recibido(paquete);
                    }

                    if (esta_Conectado())
                        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(recibir_CallBack), socket);
                    else
                        get_Desconectar_Socket();
                }
                else
                {
                    get_Desconectar_Socket();
                    return;
                }
            }
            else
            {
                get_Desconectar_Socket();
                return;
            }
        }
    }
}
