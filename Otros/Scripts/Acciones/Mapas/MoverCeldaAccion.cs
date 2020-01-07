using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas
{
    public class MoverCeldaAccion : ScriptAction
    {
        public short celda_id { get; private set; }
        public MoverCeldaAccion(short _celda_id) => celda_id = _celda_id;

        internal override Task<ResultadosAcciones> process(Account account)
        {
            Map mapa = account.game.map;
            Cell celda = mapa.GetCellFromId(celda_id);

            if (celda == null)
                return resultado_fallado;

            switch (account.game.manager.movimientos.get_Mover_A_Celda(celda, account.game.map.celdas_ocupadas()))
            {
                case ResultadoMovimientos.EXITO:
                    return resultado_procesado;

                case ResultadoMovimientos.SameCell:
                    return resultado_hecho;

                case ResultadoMovimientos.MonsterOnSun:
                    account.Logger.log_normal("MOVE", "Un monstre bloque le passage attente de 5 secondes");
                    Task.Delay(5000);
                    return process(account);

                default:
                    return resultado_fallado;
            }

        }
    }
}
