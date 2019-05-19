using Bot_Dofus_1._29._1.Otros.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Entidades.Manejadores.Recolecciones;
using Bot_Dofus_1._29._1.Otros.Mapas;
using System;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Manejadores
{
    public class Manejador : IDisposable
    {
        public Movimiento movimientos { get; private set; }
        public Recoleccion recoleccion { get; private set; }
        private bool disposed;

        public Manejador(Cuenta cuenta, Mapa mapa)
        {
            movimientos = new Movimiento(cuenta, mapa);
            recoleccion = new Recoleccion(cuenta, movimientos, mapa);
        }

        public void limoiar()
        {
            movimientos.limpiar();
            recoleccion.limpiar();
        }

        #region Zona Dispose
        ~Manejador() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                movimientos.Dispose();
            }

            movimientos = null;
            disposed = true;
        }
        #endregion
    }
}
