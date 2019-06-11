using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game
{
    public class Juego : IDisposable
    {
        public Mapa mapa { get; private set; }
        public Personaje personaje { get; private set; }
        public Manejador manejador { get; private set; }
        private bool disposed = false;

        internal Juego(Cuenta cuenta)
        {
            mapa = new Mapa();
            personaje = new Personaje(cuenta);
            manejador = new Manejador(cuenta, mapa);
        }

        #region Zona Dispose
        ~Juego() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    mapa?.Dispose();
                    personaje?.Dispose();
                    manejador?.Dispose();
                }

                mapa = null;
                personaje = null;
                manejador = null;
            }
        }
        #endregion
    }
}
