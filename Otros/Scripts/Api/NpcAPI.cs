using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;
using System.Linq;
using System;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class NpcAPI : IDisposable
    {
        private Account cuenta;
        private ActionsManager manejador_acciones;
        private bool disposed;

        public NpcAPI(Account _cuenta, ActionsManager _manejador_acciones)
        {
            cuenta = _cuenta;
            manejador_acciones = _manejador_acciones;
        }

        public bool npcBanco(int npc_id)
        {
            if (npc_id > 0 && cuenta.game.map.lista_npcs().FirstOrDefault(n => n.npc_modelo_id == npc_id) == null)
                return false;

            manejador_acciones.enqueue_Accion(new NpcBankAction(npc_id), true);
            return true;
        }

        public bool hablarNpc(int npc_id)
        {
            if (npc_id > 0 && cuenta.game.map.lista_npcs().FirstOrDefault(n => n.npc_modelo_id == npc_id) == null)
                return false;

            manejador_acciones.enqueue_Accion(new NpcAction(npc_id), true);
            return true;
        }

        public void responder(short respuesta_id) => manejador_acciones.enqueue_Accion(new RespuestaAccion(respuesta_id), true);
        public void cerrar(short respuesta_id) => manejador_acciones.enqueue_Accion(new CerrarDialogoAccion(), true);

        #region Zona Dispose
        ~NpcAPI() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                cuenta = null;
                manejador_acciones = null;
                disposed = true;
            }
        }
        #endregion
    }
}
