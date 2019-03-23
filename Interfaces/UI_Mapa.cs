using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

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
            cuenta.personaje.movimiento_pathfinding += get_Dibujar_Pathfinding;
        }

        private void get_Eventos_Mapa_Cambiado()
        {
            if(!GlobalConf.modo_ultra_perfomance)
            {
                Celda[] celdas_mapa_personaje = cuenta.personaje.mapa.celdas;
                mapa.MapaAnchura = cuenta.personaje.mapa.anchura;
                mapa.MapaAltura = cuenta.personaje.mapa.altura;
                if (celdas_mapa_personaje != null && celdas_mapa_personaje.Length > 0)
                {
                    get_Dibujar_mapa(celdas_mapa_personaje);
                }
                mapa.dibujar_Mapa();
                mapa.Invalidate();
            }
        }

        private void mapa_Control_Celda_Clic(CeldaMapa celda, MouseButtons botones)
        {
            int celda_actual = cuenta.personaje.celda_id, celda_destino = celda.id;
            if (botones == MouseButtons.Left && celda_actual != 0 && celda_destino != 0)
            {
                Task.Run(() => 
                {
                    switch (cuenta.personaje.mapa.get_Mover_Celda_Resultado(cuenta.personaje.celda_id, celda_destino, true))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.logger.log_informacion("UI_MAPA", "Personaje desplazado a la casilla: " + celda_destino);
                        break;

                        case ResultadoMovimientos.MISMA_CELDA:
                            cuenta.logger.log_Error("UI_MAPA", "El jugador está en la misma a la seleccionada");
                        break;

                        case ResultadoMovimientos.FALLO:
                        case ResultadoMovimientos.PATHFINDING_ERROR:
                            cuenta.logger.log_Error("UI_MAPA", "Error desplazando el personaje a la casilla: " + celda_destino);
                        break;
                    }
                });
            }
            else
            {
                cuenta.logger.log_Error("UI_MAPA", "Error al intentar mover el personaje" + celda_actual);
            }
        }

        private void get_Dibujar_mapa(Celda[] celdas_mapa_personaje)
        {
            for (int i = 0; i < celdas_mapa_personaje.Length; i++)
            {
                switch (celdas_mapa_personaje[i].tipo)
                {
                    case TipoCelda.NO_CAMINABLE:
                        mapa.celdas[i].Celda_Estado = CeldaEstado.NO_CAMINABLE;
                    break;

                    case TipoCelda.OBJETO_INTERACTIVO:
                        mapa.celdas[i].Celda_Estado = CeldaEstado.OBJETO_INTERACTIVO;
                    break;

                    case TipoCelda.CELDA_TELEPORT:
                        mapa.celdas[i].Celda_Estado = CeldaEstado.CELDA_TELEPORT;
                    break;

                    default:
                        mapa.celdas[i].Celda_Estado = CeldaEstado.CAMINABLE;
                    break;
                }
            }
        }

        private void get_Dibujar_Pathfinding(List<int> lista_celdas)
        {
            mapa.BeginInvoke((Action)(() =>
            {
                get_Dibujar_mapa(cuenta.personaje.mapa.celdas);
                foreach (int celda in lista_celdas)
                {
                    mapa.celdas[celda].Celda_Estado = CeldaEstado.PELEA_EQUIPO_AZUL;
                }
                mapa.Invalidate();
            }));
        }
    }
}
