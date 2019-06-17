using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;
using System;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class API : IDisposable
    {
        public InventarioApi inventario { get; private set; }
        public PersonajeApi personaje { get; private set; }
        private bool disposed;

        public API(Cuenta cuenta, ManejadorAcciones manejar_acciones)
        {
            inventario = new InventarioApi(cuenta, manejar_acciones);
            personaje = new PersonajeApi(cuenta);
        }

        #region Zona Dispose
        ~API() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    inventario.Dispose();
                }

                inventario = null;
                disposed = true;
            }
        }
        #endregion
    }
}
