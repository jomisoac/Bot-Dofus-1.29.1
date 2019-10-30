using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Inventario
{
    public class UtilizarObjetoAccion : ScriptAction
    {
        public int modelo_id { get; private set; }

        public UtilizarObjetoAccion(int _modelo_id) => modelo_id = _modelo_id;

        internal override async Task<ResultadosAcciones> process(Account cuenta)
        {
            InventoryObject objeto = cuenta.game.character.inventario.get_Objeto_Modelo_Id(modelo_id);

            if (objeto != null)
            {
                cuenta.game.character.inventario.utilizar_Objeto(objeto);
                await Task.Delay(800);
            }

            return ResultadosAcciones.HECHO;
        }
    }
}
