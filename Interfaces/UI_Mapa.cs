using System.Threading.Tasks;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

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
            if(!GlobalConf.modo_ultra_perfomance)
            {
                Celda[] celdas_mapa_personaje = cuenta.personaje.mapa.celdas;
                if (celdas_mapa_personaje != null && celdas_mapa_personaje.Length > 0)
                {
                    for (int i = 0; i < celdas_mapa_personaje.Length; i++)
                    {
                        switch (celdas_mapa_personaje[i].tipo_caminable)
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
                }
                mapa.Invalidate();
            }
        }

        private void mapa_Control_Celda_Clic(CeldaMapa celda, MouseButtons botones)
        {
            int celda_id_actual = cuenta.personaje.celda_id, celda_destino = celda.id;
            if (botones == MouseButtons.Left && celda_id_actual != 0 && celda_destino != 0)
            {
                Task.Run(() => 
                {
                    switch (cuenta.personaje.mapa.get_Mover_Celda_Resultado(celda_destino))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.logger.log_informacion("UI_MAPA", "Personaje desplazado a la casilla: " + celda_destino);
                        break;

                        case ResultadoMovimientos.FALLO:
                        case ResultadoMovimientos.PATHFINDING_ERROR:
                        case ResultadoMovimientos.MISMA_CELDA:
                            cuenta.logger.log_Error("UI_MAPA", "Error desplazando el personaje a la casilla: " + celda_destino);
                        break;
                    }
                });
            }
            else
            {
                cuenta.logger.log_Error("UI_MAPA", "Error al intentar mover el personaje");
            }
        }
    }
}
