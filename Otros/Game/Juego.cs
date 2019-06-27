using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game
{
    public class Juego : IEliminable, IDisposable
    {
        public Mapa mapa { get; private set; }
        public Personaje personaje { get; private set; }
        public Manejador manejador { get; private set; }
        public Pelea pelea{ get; private set; }
        private bool disposed = false;

        internal Juego(Cuenta cuenta)
        {
            mapa = new Mapa();
            personaje = new Personaje(cuenta);
            manejador = new Manejador(cuenta, mapa);
            pelea = new Pelea(cuenta);
        }

        public void limpiar()
        {
            mapa.limpiar();
            manejador.limpiar();
            pelea.limpiar();
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
                    mapa.Dispose();
                    personaje.Dispose();
                    manejador.Dispose();
                    pelea.Dispose();
                }

                mapa = null;
                personaje = null;
                manejador = null;
                pelea = null;
            }
        }
        #endregion
    }
}
