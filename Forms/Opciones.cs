using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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
            checkBox_modo_perfomance.Checked = GlobalConf.modo_ultra_perfomance;
            textBox_ip_servidor.Text = GlobalConf.ip_conexion;
        }

        private void boton_opciones_guardar_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(textBox_ip_servidor.Text, out IPAddress address))
            {
                GlobalConf.mostrar_mensajes_debug = checkBox_mensajes_debug.Checked;
                GlobalConf.modo_ultra_perfomance = checkBox_modo_perfomance.Checked;
                GlobalConf.ip_conexion = textBox_ip_servidor.Text;
                GlobalConf.guardar_Configuracion();
                Close();
            }
            else
            {
                textBox_ip_servidor.BackColor = Color.Red;
                return;
            }

        }
    }
}
