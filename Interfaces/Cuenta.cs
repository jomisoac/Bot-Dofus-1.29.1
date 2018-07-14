using System;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Protocolo;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class Cuenta : UserControl
    {
        public CuentaConfiguracion configuracion_cuenta;
        private Cuenta cuenta;

        public Cuenta(CuentaConfiguracion _configuracion_cuenta)
        {
            InitializeComponent();
            configuracion_cuenta = _configuracion_cuenta;
            desconectarOconectarToolStripMenuItem.Text = "Conectar";
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre_cuenta = configuracion_cuenta.get_Nombre_cuenta();

            if (FormPrincipal.get_Paginas_Cuentas_Cargadas().ContainsKey(nombre_cuenta))
            {
                FormPrincipal.get_Paginas_Cuentas_Cargadas()[nombre_cuenta].contenido.Dispose();
                FormPrincipal.get_Paginas_Cuentas_Cargadas().Remove(nombre_cuenta);
            }
        }

        private void button_limpiar_consola_Click(object sender, EventArgs e) => richTextBox_mensajes_consola.Clear();

        private void desconectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (desconectarOconectarToolStripMenuItem.Text == "Desconectar")
            {
            }
            else
            {
                if(cuenta == null)
                {
                    ProtocoloSocket conexion = new ProtocoloSocket("213.248.126.93", 443);
                    cargar_Eventos_Sockets(conexion);
                    conexion.enviar_Paquete("1.29.1");
                }
            }
        }


        private void cargar_Eventos_Sockets(ProtocoloSocket socket)
        {
           socket.evento_paquete_entrada += debugger.paquete_Recibido;
        }
    }
}
