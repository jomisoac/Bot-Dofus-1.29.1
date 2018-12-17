using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class GestionCuentas : Form
    {
        private List<CuentaConf> cuentas_cargadas;

        public GestionCuentas()
        {
            InitializeComponent();
            cuentas_cargadas = new List<CuentaConf>();

            comboBox_Servidor.SelectedIndex = 0;
            cargar_Cuentas_Lista();
        }

        private void cargar_Cuentas_Lista()
        {
            listViewCuentas.Items.Clear();

            GlobalConf.get_Lista_Cuentas().ForEach(x =>
            {
                if(!Principal.get_Paginas_Cuentas_Cargadas().ContainsKey(x.get_Nombre_Cuenta()))
                {
                    listViewCuentas.Items.Add(x.get_Nombre_Cuenta()).SubItems.AddRange(new string[] { x.get_Servidor(), string.IsNullOrEmpty(x.get_nombre_personaje()) ? "Default" : x.get_nombre_personaje() });
                }
            });
        }

        private void boton_Agregar_Cuenta_Click(object sender, EventArgs e)
        {
            if (textBox_Nombre_Cuenta.TextLength != 0 && textBox_Password.TextLength != 0)
            {
                if (GlobalConf.get_Cuenta(textBox_Nombre_Cuenta.Text) != null)
                {
                    MessageBox.Show("Ya existe una cuenta con el nombre de cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GlobalConf.agregar_Cuenta(textBox_Nombre_Cuenta.Text, textBox_Password.Text, comboBox_Servidor.SelectedItem.ToString(), textBox_Nombre_Personaje.Text);
                cargar_Cuentas_Lista();

                textBox_Nombre_Cuenta.Clear();
                textBox_Password.Clear();
                textBox_Nombre_Personaje.Clear();

                if (checkBox_Agregar_Retroceder.Checked)
                {
                    tabControlPrincipalCuentas.SelectedIndex = 0;
                }
                GlobalConf.guardar_Configuracion();
            }
            else
            {
                tableLayoutPanel6.Controls.OfType<TableLayoutPanel>().ToList().ForEach(panel =>
                {
                    panel.Controls.OfType<TextBox>().ToList().ForEach(textbox =>
                    {
                        textbox.BackColor = string.IsNullOrEmpty(textbox.Text) ? Color.Red : Color.White;
                    });
                });
            }
        }

        private void listViewCuentas_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listViewCuentas.Columns[e.ColumnIndex].Width;
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count > 0 && listViewCuentas.FocusedItem != null)
            {
                foreach (ListViewItem cuenta in listViewCuentas.SelectedItems)
                {
                    GlobalConf.eliminar_Cuenta(cuenta.Index);
                    cuenta.Remove();
                }
                GlobalConf.guardar_Configuracion();
                cargar_Cuentas_Lista();
            }
        }

        private void conectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count > 0 && listViewCuentas.FocusedItem != null)
            {
                foreach(ListViewItem cuenta in listViewCuentas.SelectedItems)
                {
                    cuentas_cargadas.Add(GlobalConf.get_Lista_Cuentas().FirstOrDefault(f => f.get_Nombre_Cuenta() == cuenta.Text));
                }
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public List<CuentaConf> get_Cuentas_Cargadas() => cuentas_cargadas;

        private void listViewCuentas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            conectarToolStripMenuItem.PerformClick();
        }
    }
}
