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
            Form form_cuentas = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is FormCuentas);
            if (form_cuentas != null)
            {
                form_cuentas.BringToFront();
                return;
            }
            new FormCuentas().Show();
        }
    }
}
