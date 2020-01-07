using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
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
            if (!checkbox_debugger.Checked)
                return;

            try
            {
                BeginInvoke((Action)(() =>
                {
                    if (lista_paquetes.Count == 200)
                    {
                        lista_paquetes.RemoveAt(0);
                        listView.Items.RemoveAt(0);
                    }

                    lista_paquetes.Add(paquete);

                    ListViewItem objeto_lista = listView.Items.Add(DateTime.Now.ToString("HH:mm:ss"));
                    objeto_lista.BackColor = enviado ? Color.FromArgb(242, 174, 138) : Color.FromArgb(170, 196, 237);
                    objeto_lista.SubItems.Add(paquete);
                }));
            }
            catch { }

        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem?.Index == -1 || listView.SelectedItems.Count == 0)
                return;

            string paquete = lista_paquetes[listView.FocusedItem.Index];
            treeView.Nodes.Clear();

            if (PaqueteRecibido.metodos.Count == 0)
                return;

            foreach (PaqueteDatos metodo in PaqueteRecibido.metodos)
            {
                if (paquete.StartsWith(metodo.nombre_paquete))
                {
                    treeView.Nodes.Add(metodo.nombre_paquete);
                    treeView.Nodes[0].Nodes.Add(paquete.Remove(0, metodo.nombre_paquete.Length));
                    treeView.Nodes[0].Expand();
                    break;
                }
            }
        }

        private void copyToClipBoard(object sender, EventArgs e)
        {
            Clipboard.SetText(treeView.Text);
        }

        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView.Columns[e.ColumnIndex].Width;
        }

        private void button_limpiar_logs_debugger_Click(object sender, EventArgs e)
        {
            lista_paquetes.Clear();
            listView.Items.Clear();
        }
    }
}
