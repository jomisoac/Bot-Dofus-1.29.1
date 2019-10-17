using Bot_Dofus_1._29._1.Utilities.Config;
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
        private List<AccountConfig> cuentas_cargadas;

        public GestionCuentas()
        {
            InitializeComponent();
            cuentas_cargadas = new List<AccountConfig>();

            comboBox_Servidor.SelectedIndex = 0;
            cargar_Cuentas_Lista();
        }

        private void cargar_Cuentas_Lista()
        {
            listViewCuentas.Items.Clear();

            GlobalConfig.Get_Accounts_List().ForEach(x =>
            {
                if (!Principal.cuentas_cargadas.ContainsKey(x.accountUsername))
                    listViewCuentas.Items.Add(x.accountUsername).SubItems.AddRange(new string[2] { x.server, x.characterName });
            });
        }

        private void boton_Agregar_Cuenta_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.Get_Account(textBox_Nombre_Cuenta.Text) != null && GlobalConfig.show_debug_messages)
            {
                MessageBox.Show("Un compte existe déjà avec le nom du compte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                GlobalConfig.AddAccount(textBox_Nombre_Cuenta.Text, textBox_Password.Text, comboBox_Servidor.SelectedItem.ToString(), textBox_nombre_personaje.Text);
                cargar_Cuentas_Lista();

                textBox_Nombre_Cuenta.Clear();
                textBox_Password.Clear();
                textBox_nombre_personaje.Clear();

                if (checkBox_Agregar_Retroceder.Checked)
                    tabControlPrincipalCuentas.SelectedIndex = 0;

                GlobalConfig.SaveConfig();
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
                    GlobalConfig.DeleteAccount(cuenta.Index);
                    cuenta.Remove();
                }
                GlobalConfig.SaveConfig();
                cargar_Cuentas_Lista();
            }
        }

        private void conectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count > 0 && listViewCuentas.FocusedItem != null)
            {
                foreach (ListViewItem cuenta in listViewCuentas.SelectedItems)
                    cuentas_cargadas.Add(GlobalConfig.Get_Accounts_List().FirstOrDefault(f => f.accountUsername == cuenta.Text));

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public List<AccountConfig> get_Cuentas_Cargadas() => cuentas_cargadas;
        private void listViewCuentas_MouseDoubleClick(object sender, MouseEventArgs e) => conectarToolStripMenuItem.PerformClick();

        private void modificar_Cuenta(object sender, EventArgs e)
        {
            if (listViewCuentas.SelectedItems.Count == 1 && listViewCuentas.FocusedItem != null)
            {
                AccountConfig cuenta = GlobalConfig.Get_Account(listViewCuentas.SelectedItems[0].Index);

                switch (sender.ToString())
                {
                    case "Cuenta":
                        string nueva_cuenta = Interaction.InputBox($"Entrez le nouveau compte", "Modifier le compte", cuenta.accountUsername);

                        if (!string.IsNullOrEmpty(nueva_cuenta) || nueva_cuenta.Split(new char[0]).Length == 0)
                            cuenta.accountUsername = nueva_cuenta;
                    break;

                    case "Contraseña":
                        string nueva_password = Interaction.InputBox($"Entrez le nouveau mot de passe", "Changer le mot de passe", cuenta.accountPassword);

                        if (!string.IsNullOrEmpty(nueva_password) || nueva_password.Split(new char[0]).Length == 0)
                            cuenta.accountPassword = nueva_password;
                    break;

                    default:
                        string nuevo_personaje = Interaction.InputBox($"Entrez le nouveau nom du personnage", "Modifier le nom du personnage", cuenta.characterName);
                        cuenta.characterName = nuevo_personaje;
                    break;
                }

                GlobalConfig.SaveConfig();
                cargar_Cuentas_Lista();
            }
        }
    }
}
