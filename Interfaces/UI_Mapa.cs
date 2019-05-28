using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Controles.ControlMapa.Animaciones;
using Bot_Dofus_1._29._1.Controles.ControlMapa.Celdas;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Mapa : UserControl
    {
        private Cuenta cuenta = null;

        public UI_Mapa(Cuenta _cuenta)
        {
            InitializeComponent();

            cuenta = _cuenta;
            control_mapa.set_Cuenta(cuenta);
        }

        private void UI_Mapa_Load(object sender, EventArgs e)
        {
            comboBox_calidad_minimapa.SelectedIndex = (byte)control_mapa.TipoCalidad;
            
            control_mapa.clic_celda += mapa_Control_Celda_Clic;
            cuenta.juego.mapa.mapa_actualizado += get_Eventos_Mapa_Cambiado;
            cuenta.juego.mapa.entidades_actualizadas += () => control_mapa.Invalidate();
            cuenta.juego.personaje.movimiento_pathfinding_minimapa += get_Dibujar_Pathfinding;
        }

        private void get_Eventos_Mapa_Cambiado()
        {
            Mapa mapa = cuenta.juego.mapa;
            Celda[] celdas_mapa_juego = mapa.celdas;

            control_mapa.mapa_anchura = cuenta.juego.mapa.anchura;
            control_mapa.mapa_altura = cuenta.juego.mapa.altura;

            BeginInvoke((Action)(() =>
            {
                label_mapa_id.Text = "MAPA ID: " + mapa.id;
            }));
            
            control_mapa.refrescar_Mapa();
        }

        private void mapa_Control_Celda_Clic(CeldaMapa celda, MouseButtons botones)
        {
            Mapa mapa = cuenta.juego.mapa;
            Celda celda_actual = cuenta.juego.personaje.celda, celda_destino = mapa.celdas[celda.id];

            if (botones == MouseButtons.Left && celda_actual.id != 0 && celda_destino.id != 0)
            {
                ResultadoMovimientos resultado = cuenta.juego.manejador.movimientos.get_Mover_A_Celda(celda_destino, mapa.celdas_ocupadas);

                switch (resultado)
                {
                    case ResultadoMovimientos.EXITO:
                        cuenta.logger.log_informacion("UI_MAPA", "Personaje desplazado a la casilla: " + celda_destino.id);
                    break;

                    case ResultadoMovimientos.MISMA_CELDA:
                        cuenta.logger.log_Error("UI_MAPA", "El jugador está en la misma a la seleccionada");
                        break;

                    default:
                        cuenta.logger.log_Error("UI_MAPA", "Error desplazando el personaje a la casilla: " + celda_destino.id + " resultado: " + resultado);
                    break;
                }
            }
            else
            {
                cuenta.logger.log_Error("UI_MAPA", "Error al intentar mover el personaje" + celda_actual.id);
            }
        }

        private void get_Dibujar_Pathfinding(List<short> lista_celdas) => Task.Run(() =>
        {
            Mapa mapa = cuenta.juego.mapa;
            control_mapa.agregar_Animacion(cuenta.juego.personaje.id, lista_celdas, PathFinderUtil.get_Tiempo_Desplazamiento_Mapa(lista_celdas.First(), lista_celdas, mapa), TipoAnimaciones.PERSONAJE);
        });
        
        private void comboBox_calidad_minimapa_SelectedIndexChanged(object sender, EventArgs e) => control_mapa.TipoCalidad = (CalidadMapa)comboBox_calidad_minimapa.SelectedIndex;
        private void checkBox_animaciones_CheckedChanged(object sender, EventArgs e) => control_mapa.Mostrar_Animaciones = checkBox_animaciones.Checked;
        private void checkBox_mostrar_celdas_CheckedChanged(object sender, EventArgs e) => control_mapa.Mostrar_Celdas_Id = checkBox_mostrar_celdas.Checked;
    }
}
