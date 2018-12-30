using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Librerias.TCP
{
    public interface ITcpSocket : IDisposable
    {
        Socket socket { get; }
        void get_Desconectar_Socket();
        event Action<string> paquete_recibido;
        event Action<string> paquete_enviado;
        event Action<object> socket_informacion;
        event Action<object> socket_desconectado;
    }
}
