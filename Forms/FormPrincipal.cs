using Bot_Dofus_1._29._1.Controles.TabControl;
using Bot_Dofus_1._29._1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class FormPrincipal : Form
    {
        public static Dictionary<string, Pagina> paginas_cuentas;

        public FormPrincipal()
        {
            InitializeComponent();
            paginas_cuentas = new Dictionary<string, Pagina>();
        }

        private void gestionDeCuentasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (FormCuentas gestion_cuentas = new FormCuentas())
            {
                if (gestion_cuentas.ShowDialog() == DialogResult.OK)
                {
                    gestion_cuentas.get_Cuentas_Cargadas().ForEach(x => 
                    {
                        paginas_cuentas.Add(x.get_Nombre_cuenta(), agregar_Nueva_Tab_Pagina(x.get_Nombre_cuenta(), new Cuenta(x)));
                    });
                }
            }
        }

        private Pagina agregar_Nueva_Tab_Pagina(string titulo, UserControl control)
        {
            control.Dock = DockStyle.Fill;

            Pagina nueva_pagina = tabControlCuentas.agregar_Nueva_Pagina(titulo);
            nueva_pagina.cabezera.propiedad_Imagen = Properties.Resources.circulo_rojo;
            nueva_pagina.cabezera.propiedad_Estado = "Desconectado";
            nueva_pagina.contenido.Controls.Add(control);

            return nueva_pagina;
        }
    }
}
