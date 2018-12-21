using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Pelea : UserControl
    {
        private Cuenta cuenta;

        public UI_Pelea(Cuenta _cuenta)
        {
            InitializeComponent();
            cuenta = _cuenta;

            refrescar_Lista_Hechizos();
            cuenta.personaje.hechizos_actualizados += actualizar_Agregar_Lista_Hechizos;
        }

        private void UI_Pelea_Load(object sender, EventArgs e)
        {
            comboBox_lista_posicionamiento.SelectedIndex = 2;
            comboBox_lista_tactica.SelectedIndex = 2;
            comboBox_focus_hechizo.SelectedIndex = 0;
        }

        private void actualizar_Agregar_Lista_Hechizos()
        {
            comboBox_lista_hechizos.Items.Clear();

            comboBox_lista_hechizos.DisplayMember = "nombre";
            comboBox_lista_hechizos.ValueMember = "id";
            comboBox_lista_hechizos.DataSource = cuenta.personaje.hechizos;

            comboBox_lista_hechizos.SelectedIndex = 0;
        }

        private void button_agregar_hechizo_Click(object sender, EventArgs e)
        {
            Hechizos hechizo = comboBox_lista_hechizos.SelectedItem as Hechizos;

            cuenta.pelea_extension.configuracion.hechizos.Add(new HechizoPelea(hechizo.id, hechizo.nombre, (HechizoFocus)comboBox_focus_hechizo.SelectedIndex, Convert.ToByte(numeric_lanzamientos_turno.Value)));
            cuenta.pelea_extension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void refrescar_Lista_Hechizos()
        {
            listView_hechizos_pelea.Items.Clear();

            cuenta.pelea_extension.configuracion.hechizos.ForEach(hechizo =>
            {
                listView_hechizos_pelea.Items.Add(hechizo.id.ToString()).SubItems.AddRange(new string[]
                {
                    hechizo.nombre, hechizo.focus.ToString(), hechizo.lanzamientos_x_turno.ToString()
                });
            });
        }

        private void button_subir_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null || listView_hechizos_pelea.FocusedItem.Index == 0)
                return;

            List<HechizoPelea> hechizo = cuenta.pelea_extension.configuracion.hechizos;
            HechizoPelea temporal = hechizo[listView_hechizos_pelea.FocusedItem.Index - 1];

            hechizo[listView_hechizos_pelea.FocusedItem.Index - 1] = hechizo[listView_hechizos_pelea.FocusedItem.Index];
            hechizo[listView_hechizos_pelea.FocusedItem.Index] = temporal;
            cuenta.pelea_extension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void button_bajar_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null || listView_hechizos_pelea.FocusedItem.Index == 0)
                return;

            List<HechizoPelea> hechizo = cuenta.pelea_extension.configuracion.hechizos;
            HechizoPelea temporal = hechizo[listView_hechizos_pelea.FocusedItem.Index + 1];

            hechizo[listView_hechizos_pelea.FocusedItem.Index + 1] = hechizo[listView_hechizos_pelea.FocusedItem.Index];
            hechizo[listView_hechizos_pelea.FocusedItem.Index] = temporal;
            cuenta.pelea_extension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }
    }
}
