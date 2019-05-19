using Bot_Dofus_1._29._1.Otros.Entidades.Manejadores.Recolecciones;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades;
using MoonSharp.Interpreter;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Manejadores
{
    public class ManejadorAcciones : IDisposable
    {
        private Cuenta cuenta;
        private ConcurrentQueue<AccionesScript> fila_acciones;
        private AccionesScript accion_actual;
        private TimerWrapper timeout_timer;
        public int contador_peleas_mapa { get; private set; }

        public event Action<bool> evento_accion_finalizada;
        private bool disposed;
        private bool mapa_cambiado;

        //contadores
        private int contador_pelea, contador_recoleccion;

        public ManejadorAcciones(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            fila_acciones = new ConcurrentQueue<AccionesScript>();
            timeout_timer = new TimerWrapper(60000, time_Out_Callback);
            Personaje personaje = cuenta.personaje;
            
            personaje.mapa.mapa_actualizado += evento_Mapa_Cambiado;
            cuenta.pelea.pelea_creada += get_Pelea_Creada;
            personaje.manejador.movimientos.movimiento_finalizado += evento_Movimiento_Celda;
            personaje.pregunta_npc_recibida += npcs_Preguntas_Recibida;
            personaje.inventario.almacenamiento_abierto += iniciar_Almacenamiento;
            personaje.inventario.almacenamiento_cerrado += cerrar_Almacenamiento;
            personaje.manejador.recoleccion.recoleccion_iniciada += get_Recoleccion_Iniciada;
            personaje.manejador.recoleccion.recoleccion_acabada += get_Recoleccion_Acabada;
        }

        private void evento_Mapa_Cambiado()
        {
            if (!cuenta.script.corriendo || accion_actual == null)
                return;

            mapa_cambiado = true;

            // cuando inicia una pelea "resetea el mapa"
            if (!(accion_actual is PeleasAccion))
                contador_peleas_mapa = 0;

            //si el bot se mete en una pelea
            if (!(accion_actual is CambiarMapaAccion) && !(accion_actual is PeleasAccion) && !(accion_actual is RecoleccionAccion))
                return;

            limpiar_Acciones();
            acciones_Salida(1500);
        }

        private async void evento_Movimiento_Celda(bool es_correcto)
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is PeleasAccion)
            {
                if (es_correcto)
                {
                    for (int delay = 0; delay < 10000 && cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO; delay += 500)
                        await Task.Delay(500);

                    if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                    {
                        cuenta.logger.log_Peligro("SCRIPT", "Error al lanzar la pelea, los monstruos pudieron haberse movido o sido robados!");
                        acciones_Salida(100);
                    }
                }
            }
            else if (accion_actual is CambiarMapaAccion && !es_correcto)
            {
                cuenta.script.detener_Script("error al mover a la celda");
            }
        }

        private void get_Recoleccion_Iniciada()
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is RecoleccionAccion)
            {
                contador_recoleccion++;

                if (cuenta.script.manejador_script.get_Global_Or("MOSTRAR_CONTADOR_RECOLECCION", DataType.Boolean, false))
                    cuenta.logger.log_informacion("SCRIPT", $"Recolección número: #{contador_recoleccion}");
            }
        }

        private void get_Recoleccion_Acabada(RecoleccionResultado resultado)
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is RecoleccionAccion)
            {
                switch (resultado)
                {
                    case RecoleccionResultado.FALLO:
                        cuenta.script.detener_Script("Error recolectando");
                    break;

                    default:
                        acciones_Salida(1000);
                    break;
                }
            }
                
        }

        private void get_Pelea_Creada()
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is PeleasAccion)
            {
                timeout_timer.Stop();
                contador_peleas_mapa++;
                contador_pelea++;

                if (cuenta.script.manejador_script.get_Global_Or("MOSTRAR_CONTADOR_PELEAS", DataType.Boolean, false))
                    cuenta.logger.log_informacion("SCRIPT", $"Combate número: #{contador_pelea}");
            }
        }

        private void npcs_Preguntas_Recibida(string lista_preguntas)
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is NpcBancoAccion nba)
            {
                if (cuenta.Estado_Cuenta != EstadoCuenta.HABLANDO)
                    return;

                string[] pregunta_separada = lista_preguntas.Split('|');
                string[] respuestas_disponibles = pregunta_separada[1].Split(';');
                int respuestas_accion = int.Parse(respuestas_disponibles[nba.respuesta_id]);

                cuenta.conexion.enviar_Paquete("DR" + pregunta_separada[0].Split(';')[0] + "|" + respuestas_accion);
            }
        }

        public void enqueue_Accion(AccionesScript accion, bool iniciar_dequeue_acciones = false)
        {
            fila_acciones.Enqueue(accion);

            if (iniciar_dequeue_acciones)
                acciones_Salida(0);
        }

        private void limpiar_Acciones()
        {
            while (fila_acciones.TryDequeue(out AccionesScript temporal)) { };
            accion_actual = null;
        }

        private void iniciar_Almacenamiento()
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is NpcBancoAccion)
                acciones_Salida(400);
        }

        private void cerrar_Almacenamiento()
        {
            if (!cuenta.script.corriendo)
                return;

            if (accion_actual is CerrarVentanaAccion)
                acciones_Salida(400);
        }

        private async Task procesar_Accion_Actual()
        {
            if (!cuenta.script.corriendo)
                return;

            string type = accion_actual.GetType().Name;

            switch (await accion_actual.proceso(cuenta))
            {
                case ResultadosAcciones.HECHO:
                    acciones_Salida(100);
                break;

                case ResultadosAcciones.FALLO:
                    cuenta.logger.log_Peligro("SCRIPT", $"{type} fallo al procesar.");
                break;

                case ResultadosAcciones.PROCESANDO:
                    timeout_timer.Start();
                break;
            }
        }

        private void time_Out_Callback(object state)
        {
            if (!cuenta.script.corriendo)
                return;

            cuenta.logger.log_Peligro("SCRIPT", "Tiempo acabado");
            cuenta.script.detener_Script();
            cuenta.script.activar_Script();
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

            if (timeout_timer.habilitado)
                timeout_timer.Stop();

            if (delay > 0)
                await Task.Delay(delay);

            if (fila_acciones.Count > 0)
            {
                if (fila_acciones.TryDequeue(out AccionesScript accion))
                {
                    accion_actual = accion;
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
            contador_recoleccion = 0;
            timeout_timer.Stop();
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
                if (disposing)
                {
                    timeout_timer.Dispose();
                }
                accion_actual = null;
                fila_acciones = null;
                cuenta = null;
                timeout_timer = null;
                disposed = true;
            }
        }
        #endregion
    }
}
