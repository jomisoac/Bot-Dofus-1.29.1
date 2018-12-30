using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Personaje : UserControl
    {
        private Cuenta cuenta;

        public UI_Personaje(Cuenta _cuenta)
        {
            InitializeComponent();
            cuenta = _cuenta;
            cuenta.personaje.personaje_seleccionado += personaje_Seleccionado_Servidor_Juego;
            cuenta.personaje.caracteristicas_actualizadas += personaje_Caracteristicas_Actualizadas;
        }

        private void personaje_Seleccionado_Servidor_Juego()
        {
            BeginInvoke((Action)(() =>
            {
                StringBuilder URL = new StringBuilder().Append("http://staticns.ankama.com/dofus/renderer/look/");
                URL.Append(Hash.encriptar_hexadecimal("{1|" + cuenta.personaje.gfxID + ",2164,3419,3429,3576,3488|1=8537887,2=0,3=0,4=13738269,5=13738269|145}")).Append("/full/1/256_450-0.png");
                imagen_personaje.LoadAsync(URL.ToString());

                label_nombre_personaje.Text = cuenta.personaje.nombre_personaje;
                label_nivel_personaje.Text = $"Nivel {cuenta.personaje.nivel}";
            }));
        }

        private void personaje_Caracteristicas_Actualizadas()
        {
            BeginInvoke((Action)(() =>
            {
                //Sumario
                label_puntos_vida.Text = cuenta.personaje.caracteristicas.vitalidad_actual.ToString() + '/' + cuenta.personaje.caracteristicas.vitalidad_maxima.ToString();
                label_puntos_accion.Text = cuenta.personaje.caracteristicas.puntos_accion.total_stats.ToString();
                label_puntos_movimiento.Text = cuenta.personaje.caracteristicas.puntos_movimiento.total_stats.ToString();
                label_iniciativa.Text = cuenta.personaje.caracteristicas.iniciativa.total_stats.ToString();
                label_prospeccion.Text = cuenta.personaje.caracteristicas.prospeccion.total_stats.ToString();
                label_alcanze.Text = cuenta.personaje.caracteristicas.alcanze.total_stats.ToString();
                label_invocaciones.Text = cuenta.personaje.caracteristicas.criaturas_invocables.total_stats.ToString();

                //Caracteristicas
                stats_vitalidad.Text = cuenta.personaje.caracteristicas.vitalidad.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.vitalidad.equipamiento.ToString() + ")";
                stats_sabiduria.Text = cuenta.personaje.caracteristicas.sabiduria.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.sabiduria.equipamiento.ToString() + ")";
                stats_fuerza.Text = cuenta.personaje.caracteristicas.fuerza.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.fuerza.equipamiento.ToString() + ")";
                stats_inteligencia.Text = cuenta.personaje.caracteristicas.inteligencia.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.inteligencia.equipamiento.ToString() + ")";
                stats_suerte.Text = cuenta.personaje.caracteristicas.suerte.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.suerte.equipamiento.ToString() + ")";
                stats_agilidad.Text = cuenta.personaje.caracteristicas.agilidad.base_personaje.ToString() + " (" + cuenta.personaje.caracteristicas.agilidad.equipamiento.ToString() + ")";

                //Otros
                label_capital_stats.Text = cuenta.personaje.puntos_caracteristicas.ToString();
            }));
        }
    }
}
