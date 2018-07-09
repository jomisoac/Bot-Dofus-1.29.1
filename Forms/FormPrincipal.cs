using System.Linq;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void gestionDeCuentasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (FormCuentas gestion_cuentas = new FormCuentas())
            {
                if (gestion_cuentas.ShowDialog() == DialogResult.OK)
                {
                    //gestion_cuentas.get_Cuentas_Cargadas().ForEach(x => x.get_Nombre_cuenta());
                }
            }
        }
    }
}
