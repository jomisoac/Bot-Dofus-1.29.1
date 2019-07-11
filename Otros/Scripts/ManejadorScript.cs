using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Personaje;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Global;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs;
using Bot_Dofus_1._29._1.Otros.Scripts.Api;
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
        private LuaManejadorScript manejador_script;
        public ManejadorAcciones manejar_acciones { get; private set; }
        private EstadoScript estado_script;
        private List<Bandera> banderas;
        private int bandera_id;
        private API api;
        private bool es_dung = false;
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
            manejar_acciones = new ManejadorAcciones(cuenta, manejador_script);
            banderas = new List<Bandera>();
            api = new API(cuenta, manejar_acciones);

            manejar_acciones.evento_accion_normal += get_Accion_Finalizada;
            manejar_acciones.evento_accion_personalizada += get_Accion_Personalizada_Finalizada;
            cuenta.juego.pelea.pelea_creada += get_Pelea_Creada;
            cuenta.juego.pelea.pelea_acabada += get_Pelea_Acabada;
        }

        public void get_Desde_Archivo(string ruta_archivo)
        {
            if (activado)
                throw new Exception("Ya se está ejecutando un script.");

            if (!File.Exists(ruta_archivo) || !ruta_archivo.EndsWith(".lua"))
                throw new Exception("Archivo no encontrado o no es válido.");

            manejador_script.cargar_Desde_Archivo(ruta_archivo, funciones_Personalizadas);
            evento_script_cargado?.Invoke(Path.GetFileNameWithoutExtension(ruta_archivo));
        }

        private void funciones_Personalizadas()
        {
            manejador_script.Set_Global("api", api);

            //no necesita coroutines
            manejador_script.Set_Global("personaje", api.personaje);

            manejador_script.Set_Global("mensaje", new Action<string>((mensaje) => cuenta.logger.log_informacion("SCRIPT", mensaje)));
            manejador_script.Set_Global("mensajeError", new Action<string>((mensaje) => cuenta.logger.log_Error("SCRIPT", mensaje)));
            manejador_script.Set_Global("detenerScript", new Action(() => detener_Script()));
            manejador_script.Set_Global("delayFuncion", new Action<int>((ms) => manejar_acciones.enqueue_Accion(new DelayAccion(ms), true)));

            manejador_script.Set_Global("estaRecolectando", (Func<bool>)cuenta.esta_recolectando);
            manejador_script.Set_Global("estaDialogando", (Func<bool>)cuenta.esta_dialogando);

            manejador_script.script.DoString(Properties.Resources.api_ayuda);
        }

        public void activar_Script()
        {
            if (activado || cuenta.esta_ocupado())
                return;

            activado = true;
            evento_script_iniciado?.Invoke();
            estado_script = EstadoScript.MOVIMIENTO;
            iniciar_Script();
        }

        public void detener_Script(string mensaje = "script pausado")
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
                Table mapas_dung = manejador_script.get_Global_Or<Table>("MAPAS_DUNG", DataType.Table, null);
                
                if (mapas_dung != null)
                {
                    IEnumerable<int> test = mapas_dung.Values.Where(m => m.Type == DataType.Number).Select(n => (int)n.Number);

                    if (test.Contains(cuenta.juego.mapa.id))
                        es_dung = true;
                }

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
                detener_Script();
            }
        });

        private async Task aplicar_Comprobaciones()
        {
            await verificar_Muerte();

            if (!corriendo)
                return;

            await get_Verificar_Script_Regeneracion();

            if (!corriendo)
                return;

            await get_Verificar_Regeneracion();

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
            if (!get_Maximos_Pods())//si no tiene el limite de pods no verificada por cada mapa
                return;

            if (!es_dung && estado_script != EstadoScript.BANCO)
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

        private void procesar_Entradas(Table valor)
        {
            banderas.Clear();
            bandera_id = 0;

            DynValue bandera = valor.Get("personalizado");
            if (!bandera.IsNil() && bandera.Type == DataType.Function)
                banderas.Add(new FuncionPersonalizada(bandera));

            if (estado_script == EstadoScript.MOVIMIENTO)
            {
                bandera = valor.Get("recolectar");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new RecoleccionBandera());

                bandera = valor.Get("pelea");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new PeleaBandera());
            }

            if (estado_script == EstadoScript.BANCO)
            {
                bandera = valor.Get("npc_banco");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new NPCBancoBandera());
            }

            bandera = valor.Get("celda");
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

            switch (bandera_actual)
            {
                case RecoleccionBandera _:
                    manejar_Recoleccion_Bandera(tiene_accion_disponible as RecoleccionAccion);
                    break;

                case PeleaBandera _:
                    manejar_Pelea_mapa(tiene_accion_disponible as PeleasAccion);
                break;

                case NPCBancoBandera _:
                    manejar_Npc_Banco_Bandera();
                break;

                case FuncionPersonalizada fp:
                    manejar_acciones.get_Funcion_Personalizada(fp.funcion);
                    break;

                case CambiarMapa mapa:
                    manejar_Cambio_Mapa(mapa);
                    break;
            }
        }

        private void manejar_Recoleccion_Bandera(RecoleccionAccion accion_recoleccion)
        {
            RecoleccionAccion accion = accion_recoleccion ?? crear_Accion_Recoleccion();

            if (accion == null)
                return;

            if (cuenta.juego.manejador.recoleccion.get_Puede_Recolectar(accion.elementos))
                manejar_acciones.enqueue_Accion(accion, true);
            else
                procesar_Actual_Bandera();
        }

        private RecoleccionAccion crear_Accion_Recoleccion()
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
            manejar_acciones.enqueue_Accion(new NpcBancoAccion(-1));
            manejar_acciones.enqueue_Accion(new AlmacenarTodosLosObjetosAccion());

            //recuperar objetos almacenados
            Table recuperar_objetos = manejador_script.get_Global_Or<Table>("BANCO_RECUPERAR_OBJETOS", DataType.Table, null);

            if (recuperar_objetos != null)
            {
                foreach (DynValue valor in recuperar_objetos.Values)
                {
                    if (valor.Type != DataType.Table)
                        continue;

                    DynValue objeto = valor.Table.Get("objeto");
                    DynValue cantidad = valor.Table.Get("cantidad");

                    if (objeto.IsNil() || objeto.Type != DataType.Number || cantidad.IsNil() || cantidad.Type != DataType.Number)
                        continue;

                    //meter enqueue
                }
            }

            manejar_acciones.enqueue_Accion(new CerrarVentanaAccion(), true);
        }

        private void manejar_Cambio_Mapa(CambiarMapa mapa)
        {
            if (CambiarMapaAccion.TryParse(mapa.celda_id, out CambiarMapaAccion accion))
                manejar_acciones.enqueue_Accion(accion, true);
            else
                detener_Script("La celda es invalida para cambiar el mapa");
        }

        private async Task get_Verificar_Sacos()
        {
            if (!manejador_script.get_Global_Or("ABRIR_SACOS", DataType.Boolean, false))
                return;

            PersonajeJuego personaje = cuenta.juego.personaje;
            List<ObjetosInventario> sacos = personaje.inventario.objetos.Where(o => o.tipo == 100).ToList();

            if (sacos.Count > 0)
            {
                foreach (ObjetosInventario saco in sacos)
                {
                    personaje.inventario.utilizar_Objeto(saco);
                    await Task.Delay(500);
                }

                cuenta.logger.log_informacion("SCRIPT", $"{sacos.Count} saco(s) abierto(s).");
            }
        }

        private void manejar_Pelea_mapa(PeleasAccion pelea_accion)
        {
            PeleasAccion accion = pelea_accion ?? get_Crear_Pelea_Accion();

            int maximas_peleas_mapa = manejador_script.get_Global_Or("PELEAS_POR_MAPA", DataType.Number, -1);
            if (maximas_peleas_mapa != -1 && manejar_acciones.contador_peleas_mapa >= maximas_peleas_mapa)
            {
                cuenta.logger.log_informacion("SCRIPT", "Alcanzado el limite de peleas en este mapa");
                procesar_Actual_Bandera();
                return;
            }

            if (!es_dung && !cuenta.juego.mapa.get_Puede_Luchar_Contra_Grupo_Monstruos(accion.monstruos_minimos, accion.monstruos_maximos, accion.monstruo_nivel_minimo, accion.monstruo_nivel_maximo, accion.monstruos_prohibidos, accion.monstruos_obligatorios))
            {
                cuenta.logger.log_informacion("SCRIPT", "Ningún grupo de monstruos disponibles en este mapa");
                procesar_Actual_Bandera();
                return;
            }

            while (es_dung && !cuenta.juego.mapa.get_Puede_Luchar_Contra_Grupo_Monstruos(accion.monstruos_minimos, accion.monstruos_maximos, accion.monstruo_nivel_minimo, accion.monstruo_nivel_maximo, accion.monstruos_prohibidos, accion.monstruos_obligatorios))
                accion = get_Crear_Pelea_Accion();

            manejar_acciones.enqueue_Accion(accion, true);
        }

        private async Task get_Verificar_Regeneracion()
        {
            if (cuenta.pelea_extension.configuracion.iniciar_regeneracion == 0)
                return;

            if (cuenta.pelea_extension.configuracion.detener_regeneracion <= cuenta.pelea_extension.configuracion.iniciar_regeneracion)
                return;

            if (cuenta.juego.personaje.caracteristicas.porcentaje_vida <= cuenta.pelea_extension.configuracion.iniciar_regeneracion)
            {
                int vida_final = cuenta.pelea_extension.configuracion.detener_regeneracion * cuenta.juego.personaje.caracteristicas.vitalidad_maxima / 100;
                int vida_para_regenerar = vida_final - cuenta.juego.personaje.caracteristicas.vitalidad_actual;

                if (vida_para_regenerar > 0)
                {
                    int tiempo_estimado = vida_para_regenerar / 2;

                    if (cuenta.Estado_Cuenta != EstadoCuenta.REGENERANDO)
                    {
                        if (cuenta.esta_ocupado())
                            return;

                        cuenta.conexion.enviar_Paquete("eU1", true);
                    }

                    cuenta.logger.log_informacion("SCRIPTS", $"Regeneración comenzada, puntos de vida a recuperar: {vida_para_regenerar}, tiempo: {tiempo_estimado} segundos.");

                    for (int i = 0; i < tiempo_estimado && cuenta.juego.personaje.caracteristicas.porcentaje_vida <= cuenta.pelea_extension.configuracion.detener_regeneracion && corriendo; i++)
                        await Task.Delay(1000);

                    if (corriendo)
                    {
                        if (cuenta.Estado_Cuenta == EstadoCuenta.REGENERANDO)
                            cuenta.conexion.enviar_Paquete("eU1", true);

                        cuenta.logger.log_informacion("SCRIPTS", "Regeneración finalizada.");
                    }
                }
            }
        }

        private async Task get_Verificar_Script_Regeneracion()
        {
            Table auto_regeneracion = manejador_script.get_Global_Or<Table>("AUTO_REGENERACION", DataType.Table, null);

            if (auto_regeneracion == null)
                return;

            PersonajeJuego personaje = cuenta.juego.personaje;
            int vida_minima = auto_regeneracion.get_Or("VIDA_MINIMA", DataType.Number, 0);
            int vida_maxima = auto_regeneracion.get_Or("VIDA_MAXIMA", DataType.Number, 100);

            if (vida_minima == 0 || personaje.caracteristicas.porcentaje_vida > vida_minima)
                return;

            int fin_vida = vida_maxima * personaje.caracteristicas.vitalidad_maxima / 100;
            int vida_para_regenerar = fin_vida - personaje.caracteristicas.vitalidad_actual;

            List<int> objetos = auto_regeneracion.Get("OBJETOS").ToObject<List<int>>();

            foreach (int id_objeto in objetos)
            {
                if (vida_para_regenerar < 20)
                    break;

                ObjetosInventario objeto = personaje.inventario.get_Objeto_Modelo_Id(id_objeto);

                if (objeto == null)
                    continue;

                if (objeto.vida_regenerada <= 0)
                    continue;

                int cantidad_necesaria = (int)Math.Floor(vida_para_regenerar / (double)objeto.vida_regenerada);
                int cantidad_correcta = Math.Min(cantidad_necesaria, objeto.cantidad);

                for (int j = 0; j < cantidad_correcta; j++)
                {
                    personaje.inventario.utilizar_Objeto(objeto);
                    await Task.Delay(800);
                }

                vida_para_regenerar -= objeto.vida_regenerada * cantidad_correcta;
            }
        }

        private void procesar_Actual_Bandera()
        {
            if (!corriendo)
                return;

            if (!es_dung && get_Maximos_Pods())
            {
                iniciar_Script();
                return;
            }

            switch (banderas[bandera_id])
            {
                case RecoleccionBandera _:
                    RecoleccionAccion accion_recoleccion = crear_Accion_Recoleccion();

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

            bandera_id++;
            if (bandera_id == banderas.Count)
                detener_Script("No se encontro ninguna acción en este mapa");
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

        private void get_Accion_Personalizada_Finalizada(bool mapa_cambiado)
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
            if (!activado)
                return;

            pausado = true;
            cuenta.juego.manejador.recoleccion.get_Cancelar_Interactivo();
        }

        private void get_Pelea_Acabada()
        {
            if (!activado)
                return;

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
                    manejador_script.Dispose();
                    api.Dispose();
                    manejar_acciones.Dispose();
                }

                manejar_acciones = null;
                manejador_script = null;
                api = null;
                activado = false;
                pausado = false;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
