using System.Net.Sockets;
using System.Text;

namespace Bot_Dofus_1._29._1.Protocolo
{
    class EstadoSocket
    {
        public Socket socket = null;
        public byte[] buffer = new byte[256];
        public StringBuilder sb = new StringBuilder();
    }
}
