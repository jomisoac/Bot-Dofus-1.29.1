using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas
{
    public class MoverCeldaAccion : AccionesScript
    {
        public short celda_id { get; private set; }
        public MoverCeldaAccion(short _celda_id) => celda_id = _celda_id;

        internal override Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            Mapa mapa = cuenta.juego.mapa;
            Celda celda = mapa.get_Celda_Id(celda_id);

            if (celda == null)
                return resultado_fallado;

            switch (cuenta.juego.manejador.movimientos.get_Mover_A_Celda(celda, cuenta.juego.mapa.celdas_ocupadas()))
            {
                case ResultadoMovimientos.EXITO:
                    return resultado_procesado;

                case ResultadoMovimientos.MISMA_CELDA:
                    return resultado_hecho;

                default:
                    return resultado_fallado;
            }

        }
    }
}
