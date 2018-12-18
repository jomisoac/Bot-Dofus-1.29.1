using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas
{
    public class CambiarMapaAccion : AccionesScript
    {
        public int celda_id { get; private set; }

        public CambiarMapaAccion(int _celda_id)
        {
            celda_id = _celda_id;
        }

        internal override Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            switch (cuenta.personaje.mapa.get_Mover_Celda_Resultado(celda_id))
            {
                case ResultadoMovimientos.EXITO:
                    return resultado_procesado;

                case ResultadoMovimientos.PATHFINDING_ERROR:
                case ResultadoMovimientos.MISMA_CELDA:
                    return resultado_hecho;

                case ResultadoMovimientos.FALLO:
                default:
                    return resultado_fallado;
            }
        }

        public static bool TryParse(string texto, out CambiarMapaAccion accion)
        {
            string[] partes = texto.Split('|');
            string randomPart = partes[Randomize.get_Random_Int(0, partes.Length)];

            Match m = Regex.Match(randomPart, @"(?<celda>\d{1,3})");
            if (m.Success)
            {
                accion = new CambiarMapaAccion(int.Parse(m.Groups["cellId"].Value));
                return true;
            }
            accion = null;
            return false;
        }
    }
}
