using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Game.Character.Spells;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Pelea : UserControl
    {
        private Account cuenta;

        public UI_Pelea(Account _cuenta)
        {
            InitializeComponent();
            cuenta = _cuenta;

            refrescar_Lista_Hechizos();
            cuenta.game.character.hechizos_actualizados += actualizar_Agregar_Lista_Hechizos;
        }

        private void UI_Pelea_Load(object sender, EventArgs e)
        {
            comboBox_focus_hechizo.SelectedIndex = 0;
            checkbox_espectadores.Checked = cuenta.fightExtension.configuracion.desactivar_espectador;

            if (cuenta.canUseMount)
                checkBox_utilizar_dragopavo.Checked = cuenta.fightExtension.configuracion.utilizar_dragopavo;
            else
                checkBox_utilizar_dragopavo.Enabled = false;

            comboBox_lista_tactica.SelectedIndex = (byte)cuenta.fightExtension.configuracion.tactica;
            comboBox_lista_posicionamiento.SelectedIndex = (byte)cuenta.fightExtension.configuracion.posicionamiento;
            comboBox_modo_lanzamiento.SelectedIndex = 0;
            numericUp_regeneracion1.Value = cuenta.fightExtension.configuracion.iniciar_regeneracion;
            numericUp_regeneracion2.Value = cuenta.fightExtension.configuracion.detener_regeneracion;
        }

        private void actualizar_Agregar_Lista_Hechizos()
        {
            comboBox_lista_hechizos.DisplayMember = "nombre";
            comboBox_lista_hechizos.ValueMember = "id";
            comboBox_lista_hechizos.DataSource = cuenta.game.character.hechizos.Values.ToList();

            if(cuenta.game.character.hechizos.Values.ToList().Count > 0)
                comboBox_lista_hechizos.SelectedIndex = 0;
        }

        private void button_agregar_hechizo_Click(object sender, EventArgs e)
        {
            Spell hechizo = comboBox_lista_hechizos.SelectedItem as Spell;
            cuenta.fightExtension.configuracion.hechizos.Add(new HechizoPelea(hechizo.id, hechizo.nombre, (HechizoFocus)comboBox_focus_hechizo.SelectedIndex, (MetodoLanzamiento)comboBox_modo_lanzamiento.SelectedIndex, Convert.ToByte(numeric_lanzamientos_turno.Value)));
            cuenta.fightExtension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void refrescar_Lista_Hechizos()
        {
            listView_hechizos_pelea.Items.Clear();

            foreach(HechizoPelea hechizo in cuenta.fightExtension.configuracion.hechizos)
            {
                listView_hechizos_pelea.Items.Add(hechizo.id.ToString()).SubItems.AddRange(new string[4]
                {
                    hechizo.nombre, hechizo.focus.ToString(), hechizo.lanzamientos_x_turno.ToString(), hechizo.metodo_lanzamiento.ToString()
                });
            }
        }

        private void button_subir_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null || listView_hechizos_pelea.FocusedItem.Index == 0)
                return;

            List<HechizoPelea> hechizo = cuenta.fightExtension.configuracion.hechizos;
            HechizoPelea temporal = hechizo[listView_hechizos_pelea.FocusedItem.Index - 1];

            hechizo[listView_hechizos_pelea.FocusedItem.Index - 1] = hechizo[listView_hechizos_pelea.FocusedItem.Index];
            hechizo[listView_hechizos_pelea.FocusedItem.Index] = temporal;
            cuenta.fightExtension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void button_bajar_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null || listView_hechizos_pelea.FocusedItem.Index == 0)
                return;

            List<HechizoPelea> hechizo = cuenta.fightExtension.configuracion.hechizos;
            HechizoPelea temporal = hechizo[listView_hechizos_pelea.FocusedItem.Index + 1];

            hechizo[listView_hechizos_pelea.FocusedItem.Index + 1] = hechizo[listView_hechizos_pelea.FocusedItem.Index];
            hechizo[listView_hechizos_pelea.FocusedItem.Index] = temporal;
            cuenta.fightExtension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Map mapa = cuenta.game.map;

            List<Monstruos> monstruos = cuenta.game.map.lista_monstruos();

            if (monstruos.Count > 0)
            {
                Cell celda_actual = cuenta.game.character.celda, celda_monstruo_destino = monstruos[0].celda;

                if (celda_actual.cellId != celda_monstruo_destino.cellId & celda_monstruo_destino.cellId > 0)
                {
                    cuenta.Logger.LogInfo("UI_PELEAS", "Monstruo encontrado en la casilla " + celda_monstruo_destino.cellId);

                    switch (cuenta.game.manager.movimientos.get_Mover_A_Celda(celda_monstruo_destino, new List<Cell>()))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.Logger.LogInfo("UI_PELEAS", "Desplazando para comenzar el combate");
                        break;

                        case ResultadoMovimientos.SameCell:
                        case ResultadoMovimientos.FALLO:
                        case ResultadoMovimientos.PathfindingError:
                            cuenta.Logger.LogError("UI_PELEAS", "El monstruo no esta en la casilla selecciona");
                        break;
                    }
                }
            }
            else
                cuenta.Logger.LogError("PELEAS", "No hay monstruos disponibles en el mapa");
        }

        private void checkbox_espectadores_CheckedChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.desactivar_espectador = checkbox_espectadores.Checked;
            cuenta.fightExtension.configuracion.guardar();
        }

        private void checkBox_utilizar_dragopavo_CheckedChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.utilizar_dragopavo = checkBox_utilizar_dragopavo.Checked;
            cuenta.fightExtension.configuracion.guardar();
        }

        private void comboBox_lista_tactica_SelectedIndexChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.tactica = (Tactica)comboBox_lista_tactica.SelectedIndex;
            cuenta.fightExtension.configuracion.guardar();
        }

        private void button_eliminar_hechizo_Click(object sender, EventArgs e)
        {
            if (listView_hechizos_pelea.FocusedItem == null)
                return;

            cuenta.fightExtension.configuracion.hechizos.RemoveAt(listView_hechizos_pelea.FocusedItem.Index);
            cuenta.fightExtension.configuracion.guardar();
            refrescar_Lista_Hechizos();
        }

        private void comboBox_lista_posicionamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.posicionamiento = (PosicionamientoInicioPelea)comboBox_lista_posicionamiento.SelectedIndex;
            cuenta.fightExtension.configuracion.guardar();
        }

        private void NumericUp_regeneracion1_ValueChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.iniciar_regeneracion = (byte)numericUp_regeneracion1.Value;
            cuenta.fightExtension.configuracion.guardar();
        }

        private void NumericUp_regeneracion2_ValueChanged(object sender, EventArgs e)
        {
            cuenta.fightExtension.configuracion.detener_regeneracion = (byte)numericUp_regeneracion2.Value;
            cuenta.fightExtension.configuracion.guardar();
        }
    }
}
