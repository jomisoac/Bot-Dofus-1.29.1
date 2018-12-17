using System;
using System.Net.Sockets;

namespace Bot_Dofus_1._29._1.LibreriaSockets.Proxy
{
    internal class ProxySocket : Socket
    {
        private string proxy_usuario { get; set; } = null;
        public string proxy_password { get; set; } = null;
        private Exception error { get; set; } = null;
        private TipoProxys tipo_proxy { get; set; } = TipoProxys.NINGUNO;

        public ProxySocket(AddressFamily familia, SocketType tipo_socket, ProtocolType tipo_procolo) : this(familia, tipo_socket, tipo_procolo, string.Empty) { }
        public ProxySocket(AddressFamily familia, SocketType tipo_socket, ProtocolType tipo_procolo, string proxy_usuario) : this(familia, tipo_socket, tipo_procolo, proxy_usuario, string.Empty) { }

        public ProxySocket(AddressFamily familia, SocketType tipo_socket, ProtocolType tipo_procolo, string _proxy_usuario, string proxyPassword) : base(familia, tipo_socket, tipo_procolo)
        {
            proxy_usuario = _proxy_usuario;
            proxy_password = proxyPassword ?? throw new ArgumentNullException(nameof(proxyPassword));
            error = new InvalidOperationException();
        }
    }
}
