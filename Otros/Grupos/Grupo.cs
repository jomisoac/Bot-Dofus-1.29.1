using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Bot_Dofus_1._29._1.Otros.Grupos
{
    public class Grupo : IDisposable
    {
        private Agrupamiento agrupamiento;
        private Dictionary<Account, ManualResetEvent> cuentas_acabadas;

        public Account lider { get; private set; }
        public ObservableCollection<Account> members { get; private set; }
        private bool disposed;

        public Grupo(Account _lider)
        {
            agrupamiento = new Agrupamiento(this);
            cuentas_acabadas = new Dictionary<Account, ManualResetEvent>();
            lider = _lider;
            members = new ObservableCollection<Account>();

            lider.group = this;
        }

        public void agregar_Miembro(Account miembro)
        {
            if (members.Count >= 7)//dofus solo se pueden 8 personaje en un grupo
                return;

            miembro.group = this;
            members.Add(miembro);
            cuentas_acabadas.Add(miembro, new ManualResetEvent(false));
        }

        public void eliminar_Miembro(Account miembro) => members.Remove(miembro);

        public void conectar_Cuentas()
        {
            lider.Connect();

            foreach (Account miembro in members)
                miembro.Connect();
        }

        public void desconectar_Cuentas()
        {
            foreach (Account miembro in members)
                miembro.Disconnect();
        }

        #region Acciones
        public void enqueue_Acciones_Miembros(ScriptAction accion, bool iniciar_dequeue = false)
        {
            if (accion is PeleasAccion)
            {
                foreach (Account miembro in members)
                    cuentas_acabadas[miembro].Set();
                return;
            }

            foreach (Account miembro in members)
                miembro.script.actions_manager.enqueue_Accion(accion, iniciar_dequeue);

            if (iniciar_dequeue)
            {
                for (int i = 0; i < members.Count; i++)
                    cuentas_acabadas[members[i]].Reset();
            }
        }

        public void esperar_Acciones_Terminadas() => WaitHandle.WaitAll(cuentas_acabadas.Values.ToArray());

        private void miembro_Acciones_Acabadas(Account cuenta)
        {
            cuenta.Logger.LogInfo("GROUPE", "Actions terminées");
            cuentas_acabadas[cuenta].Set();
        }
        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Grupo() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    agrupamiento.Dispose();
                    lider.Dispose();

                    for (int i = 0; i < members.Count; i++)
                    {
                        members[i].Dispose();
                    }
                }

                agrupamiento = null;
                cuentas_acabadas.Clear();
                cuentas_acabadas = null;
                members.Clear();
                members = null;
                lider = null;

                disposed = true;
            }
        }
        #endregion
    }
}