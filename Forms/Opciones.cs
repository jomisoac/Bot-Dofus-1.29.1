using System;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class Opciones : Form
    {
        public Opciones()
        {
            InitializeComponent();

            checkBox_mensajes_debug.Checked = GlobalConf.mostrar_mensajes_debug;
        }

        private void boton_opciones_guardar_Click(object sender, EventArgs e)
        {
            GlobalConf.mostrar_mensajes_debug = checkBox_mensajes_debug.Checked;
            GlobalConf.guardar_Configuracion();
            Close();
        }
    }
}
