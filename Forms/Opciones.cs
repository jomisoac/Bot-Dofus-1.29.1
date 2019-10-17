using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Utilities.Config;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Forms
{
    public partial class Opciones : Form
    {
        public Opciones()
        {
            InitializeComponent();

            checkBox_mensajes_debug.Checked = GlobalConfig.show_debug_messages;
            textBox_ip_servidor.Text = GlobalConfig.loginIP;
            textBox_puerto_servidor.Text = Convert.ToString(GlobalConfig.loginPort);
        }

        private void boton_opciones_guardar_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(textBox_ip_servidor.Text, out IPAddress address))
            {
                textBox_ip_servidor.BackColor = Color.Red;
                return;
            }

            if (!short.TryParse(textBox_puerto_servidor.Text, out short puerto))
            {
                textBox_puerto_servidor.BackColor = Color.Red;
                return;
            }

            GlobalConfig.show_debug_messages = checkBox_mensajes_debug.Checked;
            GlobalConfig.loginIP = textBox_ip_servidor.Text;
            GlobalConfig.loginPort = short.Parse(textBox_puerto_servidor.Text);
            GlobalConfig.SaveConfig();
            Close();
        }
    }
}
