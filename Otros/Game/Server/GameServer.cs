using Bot_Dofus_1._29._1.Otros.Enums;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game.Server
{
    public class GameServer : IEliminable, IDisposable
    {
        public int id;
        public string nombre;
        public ServerStates estado;
        private bool disposed = false;

        public GameServer() => actualizar_Datos(0, "UNDEFINED", ServerStates.OFFLINE);

        public void actualizar_Datos(int _id, string _nombre, ServerStates _estado)
        {
            id = _id;
            nombre = _nombre;
            estado = _estado;
        }

        public string GetState(ServerStates state)
        {
            switch (state)
            {
                case ServerStates.ONLINE:
                    return "En-Ligne";
                case ServerStates.SAVING:
                    return "Sauvegarde";
                case ServerStates.OFFLINE:
                    return "Hors-Ligne";
                default:
                    return "";
            }
        }

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~GameServer() => Dispose(false);

        public void Clear()
        {
            id = 0;
            nombre = null;
            estado = ServerStates.OFFLINE;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            id = 0;
            nombre = null;
            estado = ServerStates.OFFLINE;
            disposed = true;
        }
        #endregion
    }
}
