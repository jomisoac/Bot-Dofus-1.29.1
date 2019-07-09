using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento
{
    class AlmacenarTodosLosObjetosAccion : AccionesScript
    {
        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            InventarioGeneral inventario = cuenta.juego.personaje.inventario;
            
            foreach (ObjetosInventario objeto in inventario.objetos)
            {
                if(!objeto.objeto_esta_equipado())
                {
                    cuenta.conexion.enviar_Paquete($"EMO+{objeto.id_inventario}|{objeto.cantidad}");
                    inventario.eliminar_Objeto(objeto, 0, false);
                    await Task.Delay(300);
                }
            }
            return ResultadosAcciones.HECHO;
        }
    }
}
