using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Global
{
    public class DelayAccion : AccionesScript
    {
        public int milisegundos { get; private set; }

        public DelayAccion(int ms) => milisegundos = ms;

        internal override async Task<ResultadosAcciones> proceso(Account cuenta)
        {
            await Task.Delay(milisegundos);
            return ResultadosAcciones.HECHO;
        }
    }
}
