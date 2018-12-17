using System;
using System.IO;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;

namespace Bot_Dofus_1._29._1.Otros.Scripts
{
    public class ManejadorScript : IDisposable
    {
        private Cuenta cuenta;
        private LuaManejadorScript manejador_script;

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
        }

        public void activar_Script()
        {
            if (activado || cuenta.esta_ocupado)
                return;

            activado = true;
            evento_script_iniciado?.Invoke();
            iniciar_Script();
        }

        public void detener_Script(string mensaje = "Script pausado")
        {
            if (!activado)
                return;

            activado = false;
            pausado = false;

            evento_script_detenido?.Invoke(mensaje);
        }

        private void iniciar_Script() => Task.Run(async () =>
        {
            if (!corriendo)
                return;
            try
            {
                await aplicar_Comprobaciones();

                detener_Script("Ninguna acción mas encontrada en el script");
            }
            catch (Exception ex)
            {
                cuenta.logger.log_Error("Script", ex.Message);
            }
        });

        private async Task aplicar_Comprobaciones()
        {
            await Task.Delay(90000);
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
