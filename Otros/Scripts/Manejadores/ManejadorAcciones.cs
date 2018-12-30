using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Peleas;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Manejadores
{
    public class ManejadorAcciones : IDisposable
    {
        private Cuenta cuenta;
        private LuaManejadorScript manejador_script;
        private ConcurrentQueue<AccionesScript> fila_acciones;
        private AccionesScript accion_actual;
        private bool disposed;
        private bool mapa_cambiado;
        public event Action<bool> evento_accion_finalizada;

        //contadores
        private int contador_pelea;

        public ManejadorAcciones(Cuenta _cuenta, LuaManejadorScript _manejador_script)
        {
            cuenta = _cuenta;
            manejador_script = _manejador_script;
            fila_acciones = new ConcurrentQueue<AccionesScript>();

            cuenta.personaje.mapa_actualizado += evento_Mapa_Cambiado;
            cuenta.pelea.pelea_creada += evento_Pelea_Creada;
            cuenta.personaje.movimiento_celda += evento_Movimiento_Celda;
        }

        private void evento_Mapa_Cambiado()
        {
            if (!cuenta.script.corriendo)
                return;

            mapa_cambiado = true;

            if (accion_actual is CambiarMapaAccion || accion_actual is PeleasAccion)
            {
                limpiar_Acciones();
                acciones_Salida(1500);
            }
        }

        private async void evento_Movimiento_Celda(bool es_correcto)
        {
            if (cuenta.script.corriendo)
            {
                if (accion_actual is PeleasAccion)
                {
                    if (es_correcto)
                    {
                        for (int delay = 0; delay < 6000 && cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO; delay += 500)
                        {
                            await Task.Delay(500);
                        }
                        if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                        {
                            cuenta.logger.log_Peligro("Scripts", "Error al lanzar la pelea, los monstruos pudieron haberse movido o sido robados!");
                            acciones_Salida(0);
                        }
                    }
                    else
                    {
                        cuenta.script.detener_Script($"El movimiento al grupo de monstruos fracasó");
                    }
                }
            }
        }

        private void evento_Pelea_Creada()
        {
            if (cuenta.script.corriendo)
            {
                if (accion_actual is PeleasAccion)
                {
                    contador_pelea++;
                    if (manejador_script.get_Global_Or("MOSTRAR_CONTRADOR_PELEAS", DataType.Boolean, false))
                    {
                        cuenta.logger.log_informacion("SCRIPT", $"Combate numero: #{contador_pelea}");
                    }
                }
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

        public void get_Borrar_Todo()
        {
            limpiar_Acciones();
            accion_actual = null;
            contador_pelea = 0;
        }

        #region Zona Dispose
        ~ManejadorAcciones() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                accion_actual = null;
                manejador_script = null;
                fila_acciones = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
