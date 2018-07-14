using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class Debugger : UserControl
    {
        private List<string> lista_paquetes;

        public Debugger()
        {
            InitializeComponent();
            lista_paquetes = new List<string>();
        }

        public void paquete_Recibido(string paquete)
        {
            agregar_nuevo_paquete(paquete, false);
        }

        private void agregar_nuevo_paquete(string paquete, bool enviado)
        {
            if (Debugger_activado.Checked)
            {
                BeginInvoke((Action)(() =>
                {
                    if (lista_paquetes.Count == 50)
                    {
                        lista_paquetes.RemoveAt(0);
                        listView1.Items.RemoveAt(0);
                    }

                    lista_paquetes.Add(paquete);
                    ListViewItem nuevo_objeto_lista = listView1.Items.Add(DateTime.Now.ToString("HH:mm:ss"));
                    nuevo_objeto_lista.BackColor = enviado ? Color.FromArgb(242, 174, 138) : Color.FromArgb(170, 196, 237);
                    nuevo_objeto_lista.SubItems.Add(paquete);
                }));
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.FocusedItem?.Index == -1)
                return;

            treeView.Nodes.Clear();
            string message = lista_paquetes[listView1.FocusedItem.Index];
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void button_limpiar_logs_debugger_Click(object sender, EventArgs e)
        {
            lista_paquetes.Clear();
            listView1.Items.Clear();
        }
    }
}
