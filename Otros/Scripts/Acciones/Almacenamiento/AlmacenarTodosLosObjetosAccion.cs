using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento
{
    class AlmacenarTodosLosObjetosAccion : AccionesScript
    {
        internal override async Task<ResultadosAcciones> proceso(Account cuenta)
        {
            InventoryClass inventario = cuenta.game.character.inventario;
            
            foreach (InventoryObject objeto in inventario.objetos)
            {
                if(!objeto.objeto_esta_equipado())
                {
                    cuenta.connexion.SendPacket($"EMO+{objeto.id_inventario}|{objeto.cantidad}");
                    inventario.eliminar_Objeto(objeto, 0, false);
                    await Task.Delay(300);
                }
            }
            return ResultadosAcciones.HECHO;
        }
    }
}
