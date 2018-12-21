using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Manejadores
{
    public class ManejadorAcciones : IDisposable
    {
        private Cuenta cuenta;
        private LuaManejadorScript manejador_script;
        private ConcurrentQueue<AccionesScript> fila_acciones;
        private AccionesScript accion_actual;

        private bool mapa_cambiado;

        public event Action<bool> evento_accion_finalizada;

        public ManejadorAcciones(Cuenta _cuenta, LuaManejadorScript _manejador_script)
        {
            cuenta = _cuenta;
            manejador_script = _manejador_script;
            fila_acciones = new ConcurrentQueue<AccionesScript>();

            cuenta.personaje.mapa_actualizado += evento_Mapa_Cambiado;
        }

        private void evento_Mapa_Cambiado()
        {
            if (!cuenta.script.corriendo)
                return;

            mapa_cambiado = true;

            if (accion_actual is CambiarMapaAccion)
            {
                limpiar_Acciones();
                acciones_Salida(1500);
            }
        }

        public void enqueue_Accion(AccionesScript accion, bool iniciar_dequeue_acciones = false)
        {
            fila_acciones.Enqueue(accion);

            if (iniciar_dequeue_acciones)
            {
                acciones_Salida(0);
            }
        }

        private void limpiar_Acciones()
        {
            while (fila_acciones.TryDequeue(out AccionesScript temporal)) { };
            accion_actual = null;
        }

        private async Task procesar_Accion_Actual()
        {
            if (!cuenta.script.corriendo)
                return;

            if (await accion_actual.proceso(cuenta) == ResultadosAcciones.HECHO)
            {
                Console.WriteLine("asasojasoassa");
                acciones_Salida(100);
            }
        }

        private void acciones_Finalizadas()
        {
            if (mapa_cambiado)
            {
                mapa_cambiado = false;
                evento_accion_finalizada?.Invoke(true);
            }
            else
            {
                evento_accion_finalizada?.Invoke(false);
            }
        }

        private void acciones_Salida(int delay) => Task.Factory.StartNew(async () =>
        {
            if (cuenta?.script.corriendo == false)
                return;

            if (delay > 0)
            {
                await Task.Delay(delay);
            }

            if (fila_acciones.Count > 0)
            {
                if (fila_acciones.TryDequeue(out AccionesScript action))
                {
                    accion_actual = action;
                    await procesar_Accion_Actual();
                }
            }
            else
            {
                acciones_Finalizadas();
            }

        }, TaskCreationOptions.LongRunning);

        #region Zona Dispose
        ~ManejadorAcciones() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            accion_actual = null;
            manejador_script = null;
            fila_acciones = null;
            cuenta = null;
        }
        #endregion
    }
}
