using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public class PeleasAccion : ScriptAction
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

        internal override Task<ResultadosAcciones> process(Account account)
        {
            Map mapa = account.game.map;
            List<Monstruos> availableGroups = mapa.get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);

            if (availableGroups.Count > 0)
            {
                foreach (Monstruos monsterGroup in availableGroups)
                {
                    var moveResult = account.game.manager.movimientos.get_Mover_A_Celda(monsterGroup.celda, account.game.map.celdas_ocupadas().Where(c => c.cellType == CellTypes.TELEPORT_CELL).ToList());
                    switch (moveResult)
                    {
                        case ResultadoMovimientos.EXITO:
                            account.Logger.LogInfo("SCRIPT", $"Mouvent vers un groupes à la cellule : {monsterGroup.celda.cellId}, monstres total: {monsterGroup.get_Total_Monstruos}, niveaux du groupe: {monsterGroup.get_Total_Nivel_Grupo}");
                        return resultado_procesado;
                            
                        case ResultadoMovimientos.PathfindingError:
                        case ResultadoMovimientos.SameCell:
                            account.Logger.LogDanger("SCRIPT", $"Le chemin vers le groupe de monstres est bloqué. Raison : {moveResult}");
                        continue;

                        default:
                            account.script.detener_Script($"Erreur lors de la tentative de déplacement vers un groupe à la cellule : {monsterGroup.celda.cellId}. Raison : {moveResult}");
                        return resultado_fallado;
                    }
                }
            }

            return resultado_hecho;
        }
    }
}
