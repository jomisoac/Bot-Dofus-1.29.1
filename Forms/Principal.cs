using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Controles.TabControl;
using Bot_Dofus_1._29._1.Interfaces;

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class Principal : Form
    {
        public static Dictionary<string, Pagina> paginas_cuentas_cargadas;

        public Principal()
        {
            InitializeComponent();
            paginas_cuentas_cargadas = new Dictionary<string, Pagina>();
        }

        private void gestionDeCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (GestionCuentas gestion_cuentas = new GestionCuentas())
            {
                if (gestion_cuentas.ShowDialog() == DialogResult.OK)
                {
                    gestion_cuentas.get_Cuentas_Cargadas().ForEach(x =>
                    {
                        paginas_cuentas_cargadas.Add(x.get_Nombre_Cuenta(), agregar_Nueva_Tab_Pagina(x.get_Nombre_Cuenta(), new UI_Principal(x)));
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

        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Opciones of = new Opciones())
            {
                of.ShowDialog();
            }
        }
    }
}
