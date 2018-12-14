using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;

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
            cuenta.personaje.mapa_actualizado += eventos_Mapa_Cambiado;
        }

        private void eventos_Mapa_Cambiado()
        {
            Dictionary<int, Celda> celdas_mapa_personaje = cuenta.personaje.mapa.celdas;
            if (celdas_mapa_personaje != null && celdas_mapa_personaje.Count > 0)
            {
                for (int i = 0; i < celdas_mapa_personaje.Count; i++)
                {
                    switch(celdas_mapa_personaje[i].tipo_caminable)
                    {
                        case 0:
                            mapa.celdas[i].Celda_Estado = CeldaEstado.NO_CAMINABLE;
                        break;

                        case 1:
                            mapa.celdas[i].Celda_Estado = CeldaEstado.OBJETO_INTERACTIVO;
                        break;

                        case 2:
                            mapa.celdas[i].Celda_Estado = CeldaEstado.CELDA_TELEPORT;
                        break;

                        default:
                            mapa.celdas[i].Celda_Estado = CeldaEstado.CAMINABLE;
                        break;
                    }
                }
                cuenta.personaje.mapa.celda_actualizada += eventos_Celda_Actualizada;
            }
        }

        private void eventos_Celda_Actualizada(int celda_id)
        {
            //mapa.dibujar_Redonda_Celda(celda_id);
        }

        private void mapa_Control_Celda_Clic(ControlMapa control, CeldaMapa cell, MouseButtons buttons, bool hold)
        {
            if (buttons == MouseButtons.Left)
            {
                cell.Celda_Estado = CeldaEstado.CAMINABLE;
            }

            control.Invalidate(cell);
        }
    }
}
