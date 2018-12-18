using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;

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
            if (Debugger_activado.Checked && !GlobalConf.modo_ultra_perfomance)
            {
                try
                {
                    BeginInvoke((Action)(() =>
                    {
                        if (lista_paquetes.Count >= 50)
                        {
                            lista_paquetes.RemoveAt(0);
                            listView.Items.RemoveAt(0);
                        }

                        lista_paquetes.Add(paquete);
                        ListViewItem nuevo_objeto_lista = listView.Items.Add(DateTime.Now.ToString("HH:mm:ss"));
                        nuevo_objeto_lista.BackColor = enviado ? Color.FromArgb(242, 174, 138) : Color.FromArgb(170, 196, 237);
                        nuevo_objeto_lista.SubItems.Add(paquete);
                    }));
                }
                catch{}
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem?.Index == -1)
                return;

            string paquete = lista_paquetes[listView.FocusedItem.Index];

            if(treeView.Nodes != null)
                treeView.Nodes.Clear();
            if (!char.IsDigit(paquete[0]))
            {
                treeView.Nodes.Add(Extensiones.get_Enum_By_Descripcion<ListaPaquetes>(paquete.Substring(0, paquete.Length <= 3 ? 2 : 3)).ToString());
                treeView.Nodes[0].Nodes.Add(paquete);
            }
            else
            {
                treeView.Nodes.Add("VERSION_SERVIDOR");
                treeView.Nodes.Add(paquete);
            }
            treeView.Nodes[0].Expand();
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
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
