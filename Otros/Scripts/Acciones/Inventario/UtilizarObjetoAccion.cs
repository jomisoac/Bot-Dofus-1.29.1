using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Inventario
{
    public class UtilizarObjetoAccion : AccionesScript
    {
        public int modelo_id { get; private set; }

        public UtilizarObjetoAccion(int _modelo_id) => modelo_id = _modelo_id;

        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            ObjetosInventario objeto = cuenta.juego.personaje.inventario.get_Objeto_Modelo_Id(modelo_id);

            if (objeto != null)
            {
                cuenta.juego.personaje.inventario.utilizar_Objeto(objeto);
                await Task.Delay(800);
            }

            return ResultadosAcciones.HECHO;
        }
    }
}
