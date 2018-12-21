using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas;
using Bot_Dofus_1._29._1.Otros.Scripts.Banderas;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts
{
    public class ManejadorScript : IDisposable
    {
        private Cuenta cuenta;
        private LuaManejadorScript manejador_script;
        private ManejadorAcciones manejar_acciones;
        private EstadoScript estado_script;
        private List<Bandera> banderas;
        private int bandera_id;

        public bool activado { get; set; }
        public bool pausado { get; private set; }
        public bool corriendo => activado && !pausado;

        public event Action<string> evento_script_cargado;
        public event Action evento_script_iniciado;
        public event Action<string> evento_script_detenido;

        public ManejadorScript(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            manejador_script = new LuaManejadorScript();
            manejar_acciones = new ManejadorAcciones(cuenta, manejador_script);
            banderas = new List<Bandera>();
            
            manejar_acciones.evento_accion_finalizada += manejar_acciones_Acciones_Finalizadas;
        }

        public void get_Desde_Archivo(string ruta_archivo)
        {
            if (activado)
                throw new Exception("Ya se está ejecutando un script.");

            if (!File.Exists(ruta_archivo) || !ruta_archivo.EndsWith(".lua"))
                throw new Exception("Archivo no encontrado o no es válido.");

            manejador_script.cargar_Desde_Archivo(ruta_archivo, despues_De_Archivo);
            evento_script_cargado?.Invoke(Path.GetFileNameWithoutExtension(ruta_archivo));
        }

        private void despues_De_Archivo()
        {
            manejador_script.Set_Global("imprimirExito", new Action<string>((mensaje) => cuenta.logger.log_informacion("Script", mensaje)));
            manejador_script.Set_Global("imprimirError", new Action<string>((mensaje) => cuenta.logger.log_Error("Script", mensaje)));
            manejador_script.Set_Global("detenerScript", new Action(() => detener_Script()));
            
            manejador_script.Set_Global("estaRecolectando", (Func<bool>)cuenta.esta_recolectando);
            manejador_script.Set_Global("estaDialogando", (Func<bool>)cuenta.esta_dialogando);
        }

        public void activar_Script()
        {
            if (manejador_script.script != null)
            {
                if (activado || cuenta.esta_ocupado)
                    return;

                activado = true;
                evento_script_iniciado?.Invoke();
                estado_script = EstadoScript.MOVIMIENTO;
                iniciar_Script();
            }
        }

        public void detener_Script(string mensaje = "Script pausado")
        {
            if (!activado)
                return;

            activado = false;
            pausado = false;
            banderas.Clear();
            bandera_id = 0;
            evento_script_detenido?.Invoke(mensaje);
        }

        private void iniciar_Script() => Task.Run(async () =>
        {
            if (!corriendo)
                return;
            try
            {
                await aplicar_Comprobaciones();

                if (!corriendo)
                    return;

                IEnumerable<Table> entradas = manejador_script.get_Entradas_Funciones(estado_script.ToString().ToLower());
                if (entradas == null)
                {
                    detener_Script($"La función {estado_script.ToString().ToLower()} no existe");
                    return;
                }
                
                foreach (Table entrada in entradas)
                {
                    if (entrada["mapa"] == null)
                        continue;
                    
                    if (!cuenta.personaje.mapa.verificar_Mapa_Actual(int.Parse(entrada["mapa"].ToString())))
                        continue;

                    procesar_Entradas(entrada);
                    procesar_Actual_Entrada();
                    return;
                }

                detener_Script("Ninguna acción mas encontrada en el script");
            }
            catch (Exception ex)
            {
                cuenta.logger.log_Error("Script", ex.ToString());
            }
        });

        private async Task aplicar_Comprobaciones()
        {
            await verificar_Muerte();
        }

        private async Task verificar_Muerte()
        {
            if (cuenta.personaje.caracteristicas.energia_actual == 0)
            {
                cuenta.logger.log_informacion("Script", "El personaje esta muerto, pasando a modo fenix");
                estado_script = EstadoScript.FENIX;
            }
            await Task.Delay(50);
        }

        private void procesar_Entradas(Table entry)
        {
            banderas.Clear();
            bandera_id = 0;
            DynValue bandera = null;

            bandera = entry.Get("celda");
            if (!bandera.IsNil() && bandera.Type == DataType.String)
            {
                banderas.Add(new CambiarMapa(bandera.String));
            }

            if (banderas.Count == 0)
            {
                detener_Script("No se ha encontrado ninguna acción en este mapa");
            }
        }

        private void procesar_Actual_Entrada(AccionesScript alreadyParsedAction = null)
        {
            if (!corriendo)
                return;

            Bandera bandera_actual = banderas[bandera_id];

            if (bandera_actual is CambiarMapa mapa)
            {
                manejar_Cambio_Mapa(mapa);
            }
        }

        private void manejar_Cambio_Mapa(CambiarMapa mapa)
        {
            if (CambiarMapaAccion.TryParse(mapa.celda_id, out CambiarMapaAccion accion))
            {
                manejar_acciones.enqueue_Accion(accion, true);
            }
            else
            {
                detener_Script("La celda es invalida");
            }
        }

        private void procesar_Actual_Bandera(bool avoidChecks = false)
        {
            if (!corriendo)
                return;

            bandera_id++;
            if (bandera_id == banderas.Count)
            {
                detener_Script("No se ha encontrado ninguna acción en este mapa");
            }
            else
            {
                procesar_Actual_Entrada();
            }
        }

        private void manejar_acciones_Acciones_Finalizadas(bool mapa_cambiado)
        {
            if (mapa_cambiado)
            {
                iniciar_Script();
            }
            else
            {
                procesar_Actual_Bandera();
            }
        }

        #region Zona Dispose
        ~ManejadorScript() => Dispose(false);
        public void Dispose() => Dispose(true);

        public virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    manejador_script.Dispose();
                    manejar_acciones.Dispose();
                }
                manejar_acciones = null;
                manejador_script = null;
                activado = false;
                cuenta = null;
            }
            catch { }
        }
        #endregion
    }
}
