using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class API
    {
        public InventarioApi inventario { get; private set; }

        public API(Cuenta cuenta, ManejadorAcciones manejar_acciones)
        {
            inventario = new InventarioApi(cuenta, manejar_acciones);
        }
    }
}
