using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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

            mapa.clic_celda += mapa_Control_Celda_Clic;
            cuenta.personaje.mapa_actualizado += get_Eventos_Mapa_Cambiado;
            cuenta.personaje.movimiento_pathfinding_minimapa += get_Dibujar_Pathfinding;
        }

        private void get_Eventos_Mapa_Cambiado()
        {
            if (!GlobalConf.modo_ultra_perfomance)
            {
                Celda[] celdas_mapa_personaje = cuenta.personaje.mapa.celdas;
                mapa.MapaAnchura = cuenta.personaje.mapa.anchura;
                mapa.MapaAltura = cuenta.personaje.mapa.altura;

                if (celdas_mapa_personaje != null && celdas_mapa_personaje.Length > 0)
                    get_Dibujar_mapa(celdas_mapa_personaje);

                mapa.dibujar_Mapa();
                mapa.Invalidate();
            }
        }

        private async void mapa_Control_Celda_Clic(CeldaMapa celda, MouseButtons botones)
        {
            int celda_actual = cuenta.personaje.celda_id, celda_destino = celda.id;

            if (botones == MouseButtons.Left && celda_actual != 0 && celda_destino != 0)
            {
                ResultadoMovimientos resultado = await cuenta.personaje.mapa.get_Mover_Celda_Resultado(celda_actual, celda_destino, true);

                switch (resultado)
                {
                    case ResultadoMovimientos.EXITO:
                        cuenta.logger.log_informacion("UI_MAPA", "Personaje desplazado a la casilla: " + celda_destino);
                    break;

                    case ResultadoMovimientos.MISMA_CELDA:
                        cuenta.logger.log_Error("UI_MAPA", "El jugador está en la misma a la seleccionada");
                    break;

                    default:
                        cuenta.logger.log_Error("UI_MAPA", "Error desplazando el personaje a la casilla: " + celda_destino);
                    break;
                }
            }
            else
            {
                cuenta.logger.log_Error("UI_MAPA", "Error al intentar mover el personaje" + celda_actual);
            }
        }

        private void get_Dibujar_mapa(Celda[] celdas_mapa_personaje)
        {
            CeldaMapa celda_mapa = null;

            foreach (Celda celda in celdas_mapa_personaje)
            {
                celda_mapa = mapa.celdas[celda.id];

                if (celda.tipo == TipoCelda.NO_CAMINABLE && celda.objeto_interactivo != null)
                    celda_mapa.Celda_Estado = CeldaEstado.OBJETO_INTERACTIVO;
                else if (celda.tipo == TipoCelda.NO_CAMINABLE && celda.objeto_interactivo == null)
                    celda_mapa.Celda_Estado = CeldaEstado.NO_CAMINABLE;
                else if (celda.tipo == TipoCelda.OBJETO_INTERACTIVO && celda.objeto_interactivo != null)
                    celda_mapa.Celda_Estado = CeldaEstado.OBJETO_INTERACTIVO;
                else if (celda.tipo == TipoCelda.OBJETO_INTERACTIVO && celda.objeto_interactivo == null)
                    celda_mapa.Celda_Estado = CeldaEstado.NO_CAMINABLE;
                else if (celda.tipo == TipoCelda.CELDA_CAMINABLE && celda.objeto_interactivo != null)
                    celda_mapa.Celda_Estado = CeldaEstado.OBJETO_INTERACTIVO;
                else if (celda.tipo == TipoCelda.CELDA_TELEPORT)
                    celda_mapa.Celda_Estado = CeldaEstado.CELDA_TELEPORT;
                else
                    celda_mapa.Celda_Estado = CeldaEstado.CAMINABLE;
            }
        }

        private void get_Dibujar_Pathfinding(List<Nodo> lista_celdas)
        {
            mapa.BeginInvoke((Action)(() =>
            {
                get_Dibujar_mapa(cuenta.personaje.mapa.celdas);

                foreach (Nodo celda in lista_celdas)
                    mapa.celdas[celda.id].Celda_Estado = CeldaEstado.PELEA_EQUIPO_AZUL;

                mapa.Invalidate();
            }));
        }
    }
}
