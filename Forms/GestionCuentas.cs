using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
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
                if (!Principal.cuentas_cargadas.ContainsKey(x.nombre_cuenta))
                    listViewCuentas.Items.Add(x.nombre_cuenta).SubItems.AddRange(new string[2] { x.servidor, x.nombre_personaje });
            });
        }

        private void boton_Agregar_Cuenta_Click(object sender, EventArgs e)
        {
            if (GlobalConf.get_Cuenta(textBox_Nombre_Cuenta.Text) != null && GlobalConf.mostrar_mensajes_debug)
            {
                MessageBox.Show("Ya existe una cuenta con el nombre de cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool tiene_errores = false;
            tableLayoutPanel6.Controls.OfType<TableLayoutPanel>().ToList().ForEach(panel =>
            {
                panel.Controls.OfType<TextBox>().ToList().ForEach(textbox =>
                {
                    if (string.IsNullOrEmpty(textbox.Text) || textbox.Text.Split(new char[0]).Length > 1)
                    {
                        textbox.BackColor = Color.Red;
                        tiene_errores = true;
                    }
                    else
                        textbox.BackColor = Color.White;
                });
            });

            if (!tiene_errores)
            {
                GlobalConf.agregar_Cuenta(textBox_Nombre_Cuenta.Text, textBox_Password.Text, comboBox_Servidor.SelectedItem.ToString(), textBox_nombre_personaje.Text);
                cargar_Cuentas_Lista();

                textBox_Nombre_Cuenta.Clear();
                textBox_Password.Clear();
                textBox_nombre_personaje.Clear();

                if (checkBox_Agregar_Retroceder.Checked)
                    tabControlPrincipalCuentas.SelectedIndex = 0;

                GlobalConf.guardar_Configuracion();
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
                foreach (ListViewItem cuenta in listViewCuentas.SelectedItems)
                    cuentas_cargadas.Add(GlobalConf.get_Lista_Cuentas().FirstOrDefault(f => f.nombre_cuenta == cuenta.Text));

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public List<CuentaConf> get_Cuentas_Cargadas() => cuentas_cargadas;
        private void listViewCuentas_MouseDoubleClick(object sender, MouseEventArgs e) => conectarToolStripMenuItem.PerformClick();

        private void modificar_Cuenta(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count == 1 && listViewCuentas.FocusedItem != null)
            {
                CuentaConf cuenta = GlobalConf.get_Cuenta(listViewCuentas.SelectedItems[0].Index);

                switch (sender.ToString())
                {
                    case "Cuenta":
                        string nueva_cuenta = Interaction.InputBox($"Ingresa la nueva cuenta", "Modificar cuenta", cuenta.nombre_cuenta);

                        if (!string.IsNullOrEmpty(nueva_cuenta) || nueva_cuenta.Split(new char[0]).Length == 0)
                            cuenta.nombre_cuenta = nueva_cuenta;
                    break;

                    case "Contraseña":
                        string nueva_password = Interaction.InputBox($"Ingresa la nueva contraseña", "Modificar contraseña", cuenta.password);

                        if (!string.IsNullOrEmpty(nueva_password) || nueva_password.Split(new char[0]).Length == 0)
                            cuenta.password = nueva_password;
                    break;

                    default:
                        string nuevo_personaje = Interaction.InputBox($"Ingresa el nombre del nuevo personaje", "Modificar nombre de personaje", cuenta.nombre_personaje);
                        cuenta.nombre_personaje = nuevo_personaje;
                    break;
                }

                GlobalConf.guardar_Configuracion();
                cargar_Cuentas_Lista();
            }
        }
    }
}
