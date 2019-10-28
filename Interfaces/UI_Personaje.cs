using Bot_Dofus_1._29._1.Otros;
using System;
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
    public partial class UI_Personaje : UserControl
    {
        private Account cuenta;

        public UI_Personaje(Account _cuenta)
        {
            InitializeComponent();
            cuenta = _cuenta;

            ui_hechizos.set_Cuenta(cuenta);
            ui_oficios.set_Cuenta(cuenta);

            cuenta.game.character.personaje_seleccionado += personaje_Seleccionado_Servidor_Juego;
            cuenta.game.character.caracteristicas_actualizadas += personaje_Caracteristicas_Actualizadas;
         }

        private void personaje_Seleccionado_Servidor_Juego()
        {
            BeginInvoke((Action)(() =>
            {
                Bitmap imagen_raza = Properties.Resources.ResourceManager.GetObject("_" + cuenta.game.character.raza_id + cuenta.game.character.sexo) as Bitmap;
                imagen_personaje.Image = imagen_raza;

                label_nombre_personaje.Text = cuenta.game.character.nombre;
            }));
        }

        private void personaje_Caracteristicas_Actualizadas()
        {
            BeginInvoke((Action)(() =>
            {
                //Sumario
                label_puntos_vida.Text = cuenta.game.character.caracteristicas.vitalidad_actual.ToString() + '/' + cuenta.game.character.caracteristicas.vitalidad_maxima.ToString();
                label_puntos_accion.Text = cuenta.game.character.caracteristicas.puntos_accion.total_stats.ToString();
                label_puntos_movimiento.Text = cuenta.game.character.caracteristicas.puntos_movimiento.total_stats.ToString();
                label_iniciativa.Text = cuenta.game.character.caracteristicas.iniciativa.total_stats.ToString();
                label_prospeccion.Text = cuenta.game.character.caracteristicas.prospeccion.total_stats.ToString();
                label_alcanze.Text = cuenta.game.character.caracteristicas.alcanze.total_stats.ToString();
                label_invocaciones.Text = cuenta.game.character.caracteristicas.criaturas_invocables.total_stats.ToString();

                //Caracteristicas
                stats_vitalidad.Text = cuenta.game.character.caracteristicas.vitalidad.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.vitalidad.equipamiento.ToString() + ")";
                stats_sabiduria.Text = cuenta.game.character.caracteristicas.sabiduria.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.sabiduria.equipamiento.ToString() + ")";
                stats_fuerza.Text = cuenta.game.character.caracteristicas.fuerza.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.fuerza.equipamiento.ToString() + ")";
                stats_inteligencia.Text = cuenta.game.character.caracteristicas.inteligencia.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.inteligencia.equipamiento.ToString() + ")";
                stats_suerte.Text = cuenta.game.character.caracteristicas.suerte.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.suerte.equipamiento.ToString() + ")";
                stats_agilidad.Text = cuenta.game.character.caracteristicas.agilidad.base_personaje.ToString() + " (" + cuenta.game.character.caracteristicas.agilidad.equipamiento.ToString() + ")";

                //Otros
                label_capital_stats.Text = cuenta.game.character.puntos_caracteristicas.ToString();
                label_nivel_personaje.Text = $"Nivel {cuenta.game.character.nivel}";
            }));
        }
    }
}
