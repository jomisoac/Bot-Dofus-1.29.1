using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Debugger : UserControl
    {
        private List<string> lista_paquetes;

        public UI_Debugger()
        {
            InitializeComponent();
            lista_paquetes = new List<string>();
        }

        public void paquete_Recibido(string paquete)
        {
            agregar_Nuevo_Paquete(paquete, false);
        }

        public void paquete_Enviado(string paquete)
        {
            agregar_Nuevo_Paquete(paquete, true);
        }

        private void agregar_Nuevo_Paquete(string paquete, bool enviado)
        {
            if (checkbox_debugger.Checked)
            {
                BeginInvoke((Action)(() =>
                {
                    if (lista_paquetes.Count >= 50)
                    {
                        lista_paquetes.RemoveAt(0);
                        listView.Items.RemoveAt(0);
                    }

                    if (paquete.Length > 50)
                        lista_paquetes.Add(paquete.Substring(0, 50));
                    else
                        lista_paquetes.Add(paquete);

                    ListViewItem nuevo_objeto_lista = listView.Items.Add(DateTime.Now.ToString("HH:mm:ss"));
                    nuevo_objeto_lista.BackColor = enviado ? Color.FromArgb(242, 174, 138) : Color.FromArgb(170, 196, 237);
                    nuevo_objeto_lista.SubItems.Add(paquete);
                }));
            }
        }

        private void button_limpiar_logs_debugger_Click(object sender, EventArgs e)
        {
            lista_paquetes.Clear();
            listView.Items.Clear();
        }
    }
}
