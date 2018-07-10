using Bot_Dofus_1._29._1.Controles.TabControl;
using Bot_Dofus_1._29._1.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class FormPrincipal : Form
    {
        public static Dictionary<string, Pagina> paginas_cuentas_cargadas;

        public FormPrincipal()
        {
            InitializeComponent();
            paginas_cuentas_cargadas = new Dictionary<string, Pagina>();
        }

        private void gestionDeCuentasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (FormCuentas gestion_cuentas = new FormCuentas())
            {
                if (gestion_cuentas.ShowDialog() == DialogResult.OK)
                {
                    gestion_cuentas.get_Cuentas_Cargadas().ForEach(x =>
                    {
                        paginas_cuentas_cargadas.Add(x.get_Nombre_cuenta(), agregar_Nueva_Tab_Pagina(x.get_Nombre_cuenta(), new Cuenta(x)));
                    });
                }
            }
        }

        private Pagina agregar_Nueva_Tab_Pagina(string titulo, UserControl control)
        {
            Pagina nueva_pagina = tabControlCuentas.agregar_Nueva_Pagina(titulo);
            nueva_pagina.cabezera.propiedad_Imagen = Properties.Resources.circulo_rojo;
            nueva_pagina.cabezera.propiedad_Estado = "Desconectado";
            nueva_pagina.contenido.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            return nueva_pagina;
        }

        public static Dictionary<string, Pagina> get_Paginas_Cuentas_Cargadas()
        {
            return paginas_cuentas_cargadas;
        }
    }
}
