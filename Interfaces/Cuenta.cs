using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class Cuenta : UserControl
    {
        public CuentaConfiguracion configuracion_cuenta;

        public Cuenta(CuentaConfiguracion _configuracion_cuenta)
        {
            InitializeComponent();
            configuracion_cuenta = _configuracion_cuenta;
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
    }
}
