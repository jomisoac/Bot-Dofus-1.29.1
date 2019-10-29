using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Interactivos
{
    public class Interactivo : IDisposable
    {
        private Account cuenta;
        private ObjetoInteractivo interactivo_utilizado;

        public event Action<bool> fin_interactivo;
        private bool disposed;

        public Interactivo(Account _cuenta, Movimiento movimiento)
        {
            cuenta = _cuenta;
            //movimiento.movimiento_finalizado += evento_Movimiento_Finalizado;
        }

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Interactivo() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                interactivo_utilizado = null;
                cuenta = null;

                disposed = true;
            }
        }
        #endregion
    }
}
