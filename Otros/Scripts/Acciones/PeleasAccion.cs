using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public class PeleasAccion : AccionesScript
    {
        public int monstruos_minimos { get; private set; }
        public int monstruos_maximos { get; private set; }
        public int monstruo_nivel_minimo { get; private set; }
        public int monstruo_nivel_maximo { get; private set; }
        public List<int> monstruos_prohibidos { get; private set; }
        public List<int> monstruos_obligatorios { get; private set; }

        public PeleasAccion(int _monstruos_minimos, int _monstruos_maximos, int _monstruo_nivel_minimo, int _monstruo_nivel_maximo, List<int> _monstruos_prohibidos, List<int> _monstruos_obligatorios)
        {
            monstruos_minimos = _monstruos_minimos;
            monstruos_maximos = _monstruos_maximos;
            monstruo_nivel_minimo = _monstruo_nivel_minimo;
            monstruo_nivel_maximo = _monstruo_nivel_maximo;
            monstruos_prohibidos = _monstruos_prohibidos;
            monstruos_obligatorios = _monstruos_obligatorios;
        }

        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            List<Monstruo> grupos_disponibles = cuenta.personaje.mapa.get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);

            if (grupos_disponibles.Count > 0)
            {
                foreach (Monstruo grupo_monstruo in grupos_disponibles)
                {
                    switch (await cuenta.personaje.mapa.get_Mover_Celda_Resultado(cuenta.personaje.celda_id, grupo_monstruo.celda_id, false))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.logger.log_informacion("SCRIPT", $"Movimiento hacia un grupo de monstruos celda: {grupo_monstruo.celda_id}, total de monstruos: {grupo_monstruo.get_Total_Monstruos}, nivel total del grupo: {grupo_monstruo.get_Total_Nivel_Grupo}");
                        return await resultado_procesado;

                        case ResultadoMovimientos.PATHFINDING_ERROR:
                        case ResultadoMovimientos.MISMA_CELDA:
                            cuenta.logger.log_Peligro("Script", "El camino hacia el grupo de monstruos está bloqueado");
                            continue;

                        default:
                            cuenta.script.detener_Script("Movimiento hacia el grupo de monstruos erróneo");
                        return await resultado_fallado;
                    }
                }
            }
            return await resultado_hecho;
        }
    }
}
