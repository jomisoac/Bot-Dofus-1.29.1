using Bot_Dofus_1._29._1.Otros.Enums;
using System;

namespace Bot_Dofus_1._29._1.Otros.Game.Server
{
    public class GameServer : IEliminable, IDisposable
    {
        public int serverId;
        public string serverName;
        public ServerStates serverState;
        private bool _disposed = false;

        public GameServer() => RefreshData(0, "UNDEFINED", ServerStates.OFFLINE);

        public void RefreshData(int prmId, string prmServerName, ServerStates prmServerState)
        {
            serverId = prmId;
            serverName = prmServerName;
            serverState = prmServerState;
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
            serverId = 0;
            serverName = null;
            serverState = ServerStates.OFFLINE;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            serverId = 0;
            serverName = null;
            serverState = ServerStates.OFFLINE;
            _disposed = true;
        }
        #endregion
    }
}
