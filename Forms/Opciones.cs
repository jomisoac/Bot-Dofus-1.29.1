using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

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

            checkBox_mensajes_debug.Checked = GlobalConf.mostrar_mensajes_debug;
            textBox_ip_servidor.Text = GlobalConf.ip_conexion;
            textBox_puerto_servidor.Text = Convert.ToString(GlobalConf.puerto_conexion);
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

            GlobalConf.mostrar_mensajes_debug = checkBox_mensajes_debug.Checked;
            GlobalConf.ip_conexion = textBox_ip_servidor.Text;
            GlobalConf.puerto_conexion = short.Parse(textBox_puerto_servidor.Text);
            GlobalConf.guardar_Configuracion();
            Close();
        }
    }
}
