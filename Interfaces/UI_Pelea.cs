using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            comboBox_focus_hechizo.SelectedIndex = 0;

            acercarse_casillas_distancia.Value = cuenta.pelea_extension.configuracion.celdas_maximas;
            checkbox_espectadores.Checked = cuenta.pelea_extension.configuracion.desactivar_espectador;
            checkBox_utilizar_dragopavo.Checked = cuenta.pelea_extension.configuracion.utilizar_dragopavo;
            comboBox_lista_tactica.SelectedIndex = (byte)cuenta.pelea_extension.configuracion.tactica;
            comboBox_lista_posicionamiento.SelectedIndex = (byte)cuenta.pelea_extension.configuracion.posicionamiento;
        }

        private void actualizar_Agregar_Lista_Hechizos()
        {
            comboBox_lista_hechizos.DisplayMember = "nombre";
            comboBox_lista_hechizos.ValueMember = "id";
            comboBox_lista_hechizos.DataSource = cuenta.personaje.hechizos;

            comboBox_lista_hechizos.SelectedIndex = 0;
        }

        private void button_agregar_hechizo_Click(object sender, EventArgs e)
        {
            Hechizo hechizo = comboBox_lista_hechizos.SelectedItem as Hechizo;

            cuenta.pelea_extension.configuracion.hechizos.Add(new HechizoPelea(hechizo.id, hechizo.nombre, (HechizoFocus)comboBox_focus_hechizo.SelectedIndex, checkBox_lanzar_cuerpo_cuerpo.Checked, Convert.ToByte(numeric_lanzamientos_turno.Value), checkBox_AOE.Checked, checkBox_cuidado_aoe.Checked));
            cuenta.pelea_extension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void refrescar_Lista_Hechizos()
        {
            listView_hechizos_pelea.Items.Clear();

            foreach(HechizoPelea hechizo in cuenta.pelea_extension.configuracion.hechizos)
            {
                listView_hechizos_pelea.Items.Add(hechizo.id.ToString()).SubItems.AddRange(new string[]
                {
                    hechizo.nombre, hechizo.focus.ToString(), hechizo.lanzamientos_x_turno.ToString(), hechizo.lanzar_cuerpo_cuerpo ? "Si" : "No", hechizo.es_aoe ? "Si" : "No"
                });
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            Mapa mapa = cuenta.personaje.mapa;
            if (cuenta.personaje.mapa.get_Monstruos().Count > 0)
            {
                short celda_actual = cuenta.personaje.celda.id, celda_monstruo_destino = mapa.get_Monstruos().Values.ElementAt(0).celda.id;

                if (celda_actual != celda_monstruo_destino & celda_monstruo_destino != -1)
                {
                    cuenta.logger.log_informacion("PELEAS", "Monstruo encontrado en la casilla " + celda_monstruo_destino);

                    switch (mapa.get_Mover_Celda_Mapa(celda_actual, celda_monstruo_destino, false))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.logger.log_informacion("PELEAS", "Desplazando para comenzar el combate");
                        break;

                        case ResultadoMovimientos.MISMA_CELDA:
                        case ResultadoMovimientos.FALLO:
                        case ResultadoMovimientos.PATHFINDING_ERROR:
                            cuenta.logger.log_Error("PELEAS", "El monstruo no esta en la casilla selecciona");
                        break;
                    }
                }
            }
            else
                cuenta.logger.log_Error("PELEAS", "No hay monstruos disponibles en el mapa");
        }

        private void checkbox_espectadores_CheckedChanged(object sender, EventArgs e)
        {
            cuenta.pelea_extension.configuracion.desactivar_espectador = checkbox_espectadores.Checked;
            cuenta.pelea_extension.configuracion.guardar();
        }

        private void checkBox_utilizar_dragopavo_CheckedChanged(object sender, EventArgs e)
        {
            cuenta.pelea_extension.configuracion.utilizar_dragopavo = checkBox_utilizar_dragopavo.Checked;
            cuenta.pelea_extension.configuracion.guardar();
        }

        private void acercarse_casillas_distancia_ValueChanged(object sender, EventArgs e)
        {
            cuenta.pelea_extension.configuracion.celdas_maximas = Convert.ToByte(acercarse_casillas_distancia.Value);
            cuenta.pelea_extension.configuracion.guardar();
        }

        private void comboBox_lista_tactica_SelectedIndexChanged(object sender, EventArgs e)
        {
            cuenta.pelea_extension.configuracion.tactica = (Tactica)comboBox_lista_tactica.SelectedIndex;
            cuenta.pelea_extension.configuracion.guardar();
        }

        private void button_eliminar_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null)
                return;

            cuenta.pelea_extension.configuracion.hechizos.RemoveAt(listView_hechizos_pelea.FocusedItem.Index);
            cuenta.pelea_extension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void comboBox_lista_posicionamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            cuenta.pelea_extension.configuracion.posicionamiento = (PosicionamientoInicioPelea)comboBox_lista_posicionamiento.SelectedIndex;
            cuenta.pelea_extension.configuracion.guardar();
        }
    }
}
