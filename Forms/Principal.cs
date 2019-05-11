using Bot_Dofus_1._29._1.Controles.TabControl;
using Bot_Dofus_1._29._1.Interfaces;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class Principal : Form
    {
        public static Dictionary<string, Pagina> paginas_cuentas_cargadas;

        public Principal()
        {
            InitializeComponent();
            paginas_cuentas_cargadas = new Dictionary<string, Pagina>();

            if (!Directory.Exists("mapas"))
                Directory.CreateDirectory("mapas");

            if (!Directory.Exists("items"))
                Directory.CreateDirectory("items");
        }

        private void gestionDeCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (GestionCuentas gestion_cuentas = new GestionCuentas())
            {
                if (gestion_cuentas.ShowDialog() == DialogResult.OK)
                {
                   foreach (CuentaConf cuenta in gestion_cuentas.get_Cuentas_Cargadas())
                        paginas_cuentas_cargadas.Add(cuenta.nombre_cuenta, agregar_Nueva_Tab_Pagina(cuenta.nombre_cuenta, new UI_Principal(cuenta)));
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
            using (Opciones form_opciones = new Opciones())
                form_opciones.ShowDialog();
        }
    }
}
