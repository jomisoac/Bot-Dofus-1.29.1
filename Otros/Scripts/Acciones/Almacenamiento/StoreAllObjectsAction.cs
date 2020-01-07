using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento
{
    class StoreAllObjectsAction : ScriptAction
    {
        internal override async Task<ResultadosAcciones> process(Account account)
        {
            Task[] tasks = new Task[account.hasGroup ? account.group.members.Count + 1 : 1];
            tasks[0] = cleanInventory(account);

            if (account.hasGroup && account.isGroupLeader)
            {
                foreach (var follower in account.group.members)
                {
                    tasks[account.group.members.IndexOf(follower) + 1] = cleanInventory(follower);
                }
            }
            Task.WaitAll(tasks);
            return ResultadosAcciones.HECHO;
        }

        private async Task cleanInventory(Account account)
        {
            InventoryClass inventario = account.game.character.inventario;

            int idCapture = account.script.getCaptureIdAndQuantity().Key;
            int quantityCapture = account.script.getCaptureIdAndQuantity().Value;
            foreach (InventoryObject objeto in inventario.objetos)
            {
                if (!objeto.objeto_esta_equipado() && objeto.id_modelo != idCapture)
                {
                    account.connexion.SendPacket($"EMO+{objeto.id_inventario}|{objeto.cantidad}");
                    inventario.eliminar_Objeto(objeto, 0, false);
                    await Task.Delay(300);
                }
                if(objeto.id_modelo == idCapture && objeto.cantidad< quantityCapture)
                {
                    int quantite = quantityCapture - objeto.cantidad;
                    account.Logger.LogInfo("SCRIPT", $"On récupére " + quantite +" captures dans la banque");
                    account.connexion.SendPacket($"EMO-{objeto.id_inventario}|{quantite}");
                }
            }
        }
    }
}
