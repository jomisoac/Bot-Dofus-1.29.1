using System;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Grupos
{
    public class Agrupamiento : IDisposable
    {
        private Grupo grupo;
        private List<Cuenta> miembros_perdidos;
        private bool disposed;

        public Agrupamiento(Grupo _grupo) => grupo = _grupo;

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Agrupamiento() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                grupo = null;
                miembros_perdidos?.Clear();
                miembros_perdidos = null;

                disposed = true;
            }
        }
        #endregion
    }
}
