using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public abstract class AccionesScript
    {
        protected static Task<ResultadosAcciones> resultado_hecho => Task.FromResult(ResultadosAcciones.HECHO);
        protected static Task<ResultadosAcciones> resultado_procesado => Task.FromResult(ResultadosAcciones.PROCESANDO);
        protected static Task<ResultadosAcciones> resultado_fallado => Task.FromResult(ResultadosAcciones.FALLO);
        abstract internal Task<ResultadosAcciones> proceso(Cuenta cuenta);
    }

    public enum ResultadosAcciones
    {
        HECHO,
        PROCESANDO,
        FALLO
    }
}
