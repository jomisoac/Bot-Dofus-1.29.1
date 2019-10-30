using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Game.Server;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game
{
    public class GameClass : IEliminable, IDisposable
    {
        public GameServer server { get; private set; }
        public Map map { get; private set; }
        public CharacterClass character { get; private set; }
        public Manejador manager { get; private set; }
        public Fight fight { get; private set; }
        private bool _disposed = false;

        internal GameClass(Account prmAccount)
        {
            server = new GameServer();
            map = new Map();
            character = new CharacterClass(prmAccount);
            manager = new Manejador(prmAccount, map, character);
            fight = new Fight(prmAccount);
        }

        #region Zona Dispose
        ~GameClass() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void Clear()
        {
            map.Clear();
            manager.Clear();
            fight.Clear();
            character.Clear();
            server.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    map.Dispose();
                    character.Dispose();
                    manager.Dispose();
                    fight.Dispose();
                    server.Dispose();
                }

                server = null;
                map = null;
                character = null;
                manager = null;
                fight = null;
                _disposed = true;
            }
        }
        #endregion
    }
}
