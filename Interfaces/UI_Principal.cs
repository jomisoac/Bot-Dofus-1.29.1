using System;
using System.Drawing;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Logs;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Principal : UserControl
    {
        private CuentaConf configuracion_cuenta;
        private Cuenta cuenta;

        public UI_Principal(CuentaConf _configuracion_cuenta)
        {
            InitializeComponent();
            configuracion_cuenta = _configuracion_cuenta;
            desconectarOconectarToolStripMenuItem.Text = "Conectar";
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre_cuenta = configuracion_cuenta.get_Nombre_Cuenta();

            if (Principal.get_Paginas_Cuentas_Cargadas().ContainsKey(nombre_cuenta))
            {
                Principal.get_Paginas_Cuentas_Cargadas()[nombre_cuenta].contenido.Dispose();
                Principal.get_Paginas_Cuentas_Cargadas().Remove(nombre_cuenta);
            }
        }

        private void cambiar_Tab_Imagen(Image image)
        {
            if (Principal.paginas_cuentas_cargadas.ContainsKey(configuracion_cuenta.get_Nombre_Cuenta()))
            {
                Principal.paginas_cuentas_cargadas[configuracion_cuenta.get_Nombre_Cuenta()].cabezera.propiedad_Imagen = image;
            }
        }

        private void button_limpiar_consola_Click(object sender, EventArgs e) => textbox_logs.Clear();

        private void desconectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (desconectarOconectarToolStripMenuItem.Text.Equals("Conectar"))
            {
                if (cuenta == null)
                {
                    cuenta = new Cuenta(configuracion_cuenta.get_Nombre_Cuenta(), configuracion_cuenta.get_Password(), configuracion_cuenta.get_Servidor_Id());
                    cargar_Eventos_Login();
                    cuenta.evento_fase_socket += cargar_Eventos_Debugger;
                    desconectarOconectarToolStripMenuItem.Text = "Desconectar";
                }
            }
            else
            {

            }
        }

        private void cargar_Eventos_Debugger(ClienteProtocolo socket)
        {
            switch(cuenta.Fase_Socket)
            {
                case EstadoSocket.CAMBIANDO_A_JUEGO:
                    socket.evento_paquete_recibido += debugger.paquete_Recibido;
                    socket.evento_paquete_enviado += debugger.paquete_Enviado;
                    socket.evento_socket_informacion += escribir_mensaje;
                break;

                case EstadoSocket.JUEGO:
                    agregar_Tab_Pagina("Personaje", new UI_Personaje(cuenta), 2);
                break;
            }
        }

        private void cargar_Eventos_Login()
        {
            if (cuenta != null)
            {
                cuenta.conexion.evento_paquete_recibido += debugger.paquete_Recibido;
                cuenta.conexion.evento_paquete_enviado += debugger.paquete_Enviado;
                cuenta.conexion.evento_socket_informacion += escribir_mensaje;

                cuenta.evento_estado_cuenta += eventos_Estados_Cuenta;
                cuenta.logger.log_evento += (mensaje, color) => escribir_mensaje(mensaje.ToString(), color);
            }
        }

        private void eventos_Estados_Cuenta()
        {
            switch (cuenta.Estado_Cuenta)
            {
                case EstadoCuenta.DESCONECTADO:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_rojo);
                break;

                case EstadoCuenta.CONECTANDO:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_naranja);
                break;

                default:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_verde);
                break;
            }
            if (cuenta != null && Principal.paginas_cuentas_cargadas.ContainsKey(configuracion_cuenta.get_Nombre_Cuenta()))
            {
                Principal.paginas_cuentas_cargadas[configuracion_cuenta.get_Nombre_Cuenta()].cabezera.propiedad_Estado = cuenta.Estado_Cuenta.cadena_Amigable();
            }
        }

        private void escribir_mensaje(object error) => escribir_mensaje(DateTime.Now.ToString("HH:mm:ss") + " -> [Conexión] " + error, LogTipos.PELIGRO.ToString("X"));

        private void agregar_Tab_Pagina(string nombre, UserControl control, int imagen_index)
        {
            tabControl_principal.BeginInvoke((Action)(() =>
            {
                control.Dock = DockStyle.Fill;
                var newPage = new TabPage(nombre);
                newPage.ImageIndex = imagen_index;
                newPage.Controls.Add(control);
                tabControl_principal.TabPages.Add(newPage);
            }));
        }

        private void escribir_mensaje(string mensaje, string color)
        {
            if (!IsHandleCreated)
                return;

            textbox_logs.BeginInvoke((Action)(() =>
            {
                textbox_logs.Select(textbox_logs.TextLength, 0);
                textbox_logs.SelectionColor = ColorTranslator.FromHtml("#" + color);
                textbox_logs.AppendText(mensaje + Environment.NewLine);
                textbox_logs.ScrollToCaret();
            }));
        }
    }
}
