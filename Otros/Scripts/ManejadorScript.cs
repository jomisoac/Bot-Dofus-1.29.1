using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot_Dofus_1._29._1.Otros.Scripts
{
    public class ManejadorScript : IDisposable
    {
        private Cuenta cuenta;
        private LuaManejadorScript manejador_script;

        public ManejadorScript(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            manejador_script = new LuaManejadorScript();
        }

        ~ManejadorScript() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    manejador_script.Dispose();
                }
                manejador_script = null;
                cuenta = null;
            }
            catch { }
        }
    }
}
