using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Inventario;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento;
using Bot_Dofus_1._29._1.Otros.Scripts.Banderas;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts
{
    public class ManejadorScript : IDisposable
    {
        private Cuenta cuenta;
        public LuaManejadorScript manejador_script;
        private ManejadorAcciones manejar_acciones;
        private EstadoScript estado_script;
        private List<Bandera> banderas;
        private int bandera_id;
        private bool disposed;

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
            manejar_acciones = new ManejadorAcciones(cuenta);
            banderas = new List<Bandera>();

            manejar_acciones.evento_accion_finalizada += get_Accion_Finalizada;
            cuenta.pelea.pelea_creada += get_Pelea_Creada;
            cuenta.pelea.pelea_acabada += get_Pelea_Acabada;
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
            manejador_script.Set_Global("imprimir_exito", new Action<string>((mensaje) => cuenta.logger.log_informacion("Script", mensaje)));
            manejador_script.Set_Global("imprimir_error", new Action<string>((mensaje) => cuenta.logger.log_Error("Script", mensaje)));
            manejador_script.Set_Global("detener_script", new Action(() => detener_Script()));
            
            manejador_script.Set_Global("esta_recolectando", (Func<bool>)cuenta.esta_recolectando);
            manejador_script.Set_Global("esta_dialogando", (Func<bool>)cuenta.esta_dialogando);
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
            manejar_acciones.get_Borrar_Todo();
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

                    if (!cuenta.juego.mapa.esta_En_Mapa(entrada["mapa"].ToString()))
                        continue;

                    procesar_Entradas(entrada);
                    procesar_Actual_Entrada();
                    return;
                }

                detener_Script("Ninguna acción mas encontrada en el script");
            }
            catch (Exception ex)
            {
                cuenta.logger.log_Error("SCRIPT", ex.ToString());
            }
        });

        private async Task aplicar_Comprobaciones()
        {
            await verificar_Muerte();

            if (!corriendo)
                return;

            await get_Verificar_Sacos();

            if (!corriendo)
                return;

            verificar_Maximos_Pods();
        }

        private async Task verificar_Muerte()
        {
            if (cuenta.juego.personaje.caracteristicas.energia_actual == 0)
            {
                cuenta.logger.log_informacion("SCRIPT", "El personaje esta muerto, pasando a modo fenix");
                estado_script = EstadoScript.FENIX;
            }
            await Task.Delay(50);
        }

        private void verificar_Maximos_Pods()
        {
            if (get_Maximos_Pods() && estado_script != EstadoScript.BANCO)
            {
                if (!corriendo)
                    return;

                cuenta.logger.log_informacion("SCRIPT", "Inventario lleno, pasando al modo banco");
                estado_script = EstadoScript.BANCO;
            }
        }

        private bool get_Maximos_Pods()
        {
            int maxPods = manejador_script.get_Global_Or("MAXIMOS_PODS", DataType.Number, 90);
            return cuenta.juego.personaje.inventario.porcentaje_pods >= maxPods;
        }

        private void procesar_Entradas(Table entry)
        {
            banderas.Clear();
            bandera_id = 0;
            DynValue bandera = null;

            if (estado_script == EstadoScript.MOVIMIENTO)
            {
                bandera = entry.Get("recolectar");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new RecoleccionBandera());

                bandera = entry.Get("pelea");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new PeleaBandera());
            }

            if (estado_script == EstadoScript.BANCO)
            {
                bandera = entry.Get("npc_banco");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new NPCBancoBandera());
            }

            bandera = entry.Get("celda");
            if (!bandera.IsNil() && bandera.Type == DataType.String)
                banderas.Add(new CambiarMapa(bandera.String));

            if (banderas.Count == 0)
                detener_Script("no se ha encontrado ninguna acción en este mapa");
        }

        private void procesar_Actual_Entrada(AccionesScript tiene_accion_disponible = null)
        {
            if (!corriendo)
                return;

            Bandera bandera_actual = banderas[bandera_id];

            if (bandera_actual is RecoleccionBandera)
            {
                manejar_Recoleccion_Bandera(tiene_accion_disponible as RecoleccionAccion);
            }
            else if (bandera_actual is PeleaBandera)
            {
                manejar_Pelea_mapa(tiene_accion_disponible as PeleasAccion);
            }
            else if (bandera_actual is NPCBancoBandera)
            {
                manejar_Npc_Banco_Bandera();
            }
            else if (bandera_actual is CambiarMapa mapa)
            {
                manejar_Cambio_Mapa(mapa);
            }
        }

        private void manejar_Recoleccion_Bandera(RecoleccionAccion accion_recoleccion)
        {
            RecoleccionAccion accion = accion_recoleccion ?? crar_Accion_Recoleccion();

            if (accion == null)
                return;

            if (cuenta.juego.manejador.recoleccion.get_Puede_Recolectar(accion.elementos))
                manejar_acciones.enqueue_Accion(accion, true);
            else
                procesar_Actual_Bandera(true);
        }

        private RecoleccionAccion crar_Accion_Recoleccion()
        {
            Table elementos_para_recolectar = manejador_script.get_Global_Or<Table>("ELEMENTOS_RECOLECTABLES", DataType.Table, null);
            List<short> recursos_id = new List<short>();

            if (elementos_para_recolectar != null)
            {
                foreach (DynValue etg in elementos_para_recolectar.Values)
                {
                    if (etg.Type != DataType.Number)
                        continue;

                    if (cuenta.juego.personaje.get_Tiene_Skill_Id((int)etg.Number))
                        recursos_id.Add((short)etg.Number);
                }
            }

            if (recursos_id.Count == 0)
                recursos_id.AddRange(cuenta.juego.personaje.get_Skills_Recoleccion_Disponibles());

            if (recursos_id.Count == 0)
            {
                cuenta.script.detener_Script("Lista de recursos vacia, o no tienes oficios disponibles");
                return null;
            }

            return new RecoleccionAccion(recursos_id);
        }

        private void manejar_Npc_Banco_Bandera()
        {
            manejar_acciones.enqueue_Accion(new NpcBancoAccion(-1, 0));
            manejar_acciones.enqueue_Accion(new AlmacenarTodosLosObjetosAccion());
            manejar_acciones.enqueue_Accion(new CerrarVentanaAccion(), true);
        }

        private void manejar_Cambio_Mapa(CambiarMapa mapa)
        {
            if (CambiarMapaAccion.TryParse(mapa.celda_id, out CambiarMapaAccion accion))
                manejar_acciones.enqueue_Accion(accion, true);
            else
                detener_Script("La celda es invalida");
        }

        private async Task get_Verificar_Sacos()
        {
            if (!manejador_script.get_Global_Or("ABRIR_SACOS", DataType.Boolean, false))
                return;

            List<ObjetosInventario> sacos = cuenta.juego.personaje.inventario.objetos.Where(o => o.tipo == 100).ToList();

            if (sacos.Count > 0)
            {
                foreach (ObjetosInventario saco in sacos)
                {
                    cuenta.conexion.enviar_Paquete("OU" + saco.id_inventario + "|");
                    cuenta.juego.personaje.inventario.eliminar_Objetos(saco, 1, false);
                    await Task.Delay(500);
                }

                cuenta.logger.log_informacion("SCRIPT", $"{sacos.Count} saco(s) abierto(s).");
            }
        }

        private void manejar_Pelea_mapa(PeleasAccion pelea_accion)
        {
            PeleasAccion accion = pelea_accion ?? get_Crear_Pelea_Accion();

            if (cuenta.juego.mapa.get_Puede_Luchar_Contra_Grupo_Monstruos(accion.monstruos_minimos, accion.monstruos_maximos, accion.monstruo_nivel_minimo, accion.monstruo_nivel_maximo, accion.monstruos_prohibidos, accion.monstruos_obligatorios))
            {
                manejar_acciones.enqueue_Accion(accion, true);
            }
            else
            {
                cuenta.logger.log_informacion("SCRIPT", "Ningún grupo de monstruos disponibles en este mapa");
                procesar_Actual_Bandera(true);
            }
        }

        private async Task get_Verificar_Regeneracion()
        {
            Table auto_regeneracion = manejador_script.get_Global_Or<Table>("AUTO_REGENERACION", DataType.Table, null);

            if (auto_regeneracion == null)
                return;

            int vida_minima = auto_regeneracion.get_Or("vida_minima", DataType.Number, 0);
            int vida_maxima = auto_regeneracion.get_Or("vida_maxima", DataType.Number, 100);

            if (vida_minima == 0 || cuenta.juego.personaje.caracteristicas.porcentaje_vida > vida_minima)
                return;
        }

        private void procesar_Actual_Bandera(bool evitar_comprobaciones = false)
        {
            if (!corriendo)
                return;

            if (get_Maximos_Pods())
            {
                iniciar_Script();
                return;
            }

            if (!evitar_comprobaciones)
            {
                switch (banderas[bandera_id])
                {
                    case RecoleccionBandera _:
                        RecoleccionAccion accion_recoleccion = crar_Accion_Recoleccion();

                        if (cuenta.juego.manejador.recoleccion.get_Puede_Recolectar(accion_recoleccion.elementos))
                        {
                            procesar_Actual_Entrada(accion_recoleccion);
                            return;
                        }
                    break;

                    case PeleaBandera _:
                        PeleasAccion accion_pelea = get_Crear_Pelea_Accion();

                        if (cuenta.juego.mapa.get_Puede_Luchar_Contra_Grupo_Monstruos(accion_pelea.monstruos_minimos, accion_pelea.monstruos_maximos, accion_pelea.monstruo_nivel_minimo, accion_pelea.monstruo_nivel_maximo, accion_pelea.monstruos_prohibidos, accion_pelea.monstruos_obligatorios))
                        {
                            procesar_Actual_Entrada(accion_pelea);
                            return;
                        }
                    break;
                }
            }

            bandera_id++;
            if (bandera_id == banderas.Count)
                detener_Script("No se ha encontrado ninguna acción en este mapa");
            else
                procesar_Actual_Entrada();
        }

        private PeleasAccion get_Crear_Pelea_Accion()
        {
            int monstruos_minimos = manejador_script.get_Global_Or("MONSTRUOS_MINIMOS", DataType.Number, 1);
            int monstruos_maximos = manejador_script.get_Global_Or("MONSTRUOS_MAXIMOS", DataType.Number, 8);
            int monstruo_nivel_minimo = manejador_script.get_Global_Or("MINIMO_NIVEL_MONSTRUOS", DataType.Number, 1);
            int monstruo_nivel_maximo = manejador_script.get_Global_Or("MAXIMO_NIVEL_MONSTRUOS", DataType.Number, 1000);
            List<int> monstruos_prohibidos = new List<int>();
            List<int> monstruos_obligatorios = new List<int>();

            Table entrada = manejador_script.get_Global_Or<Table>("MONSTRUOS_PROHIBIDOS", DataType.Table, null);
            if (entrada != null)
            {
                foreach (var fm in entrada.Values)
                {
                    if (fm.Type != DataType.Number)
                        continue;

                    monstruos_prohibidos.Add((int)fm.Number);
                }
            }
            entrada = manejador_script.get_Global_Or<Table>("MONSTRUOS_OBLIGATORIOS", DataType.Table, null);
            if (entrada != null)
            {
                foreach (DynValue mm in entrada.Values)
                {
                    if (mm.Type != DataType.Number)
                        continue;
                    monstruos_obligatorios.Add((int)mm.Number);
                }
            }
            return new PeleasAccion(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);
        }

        private bool verificar_Acciones_Especiales()
        {
            if (estado_script == EstadoScript.BANCO && !get_Maximos_Pods())
            {
                estado_script = EstadoScript.MOVIMIENTO;
                iniciar_Script();
                return true;
            }
            return false;
        }

        #region Zona Eventos
        private void get_Accion_Finalizada(bool mapa_cambiado)
        {

            if (verificar_Acciones_Especiales())
                return;

            if (mapa_cambiado)
                iniciar_Script();
            else
                procesar_Actual_Bandera();
        }

        private void get_Pelea_Creada()
        {
            if (activado)
                pausado = true;
        }

        private void get_Pelea_Acabada()
        {
            if (activado)
                pausado = false;
        }
        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~ManejadorScript() => Dispose(false);
        
        public virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                if (disposing)
                {
                    manejador_script?.Dispose();
                    manejar_acciones?.Dispose();
                }

                manejar_acciones = null;
                manejador_script = null;
                activado = false;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
