using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs;
using Bot_Dofus_1._29._1.Utilities;
using MoonSharp.Interpreter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Utilities.Extensions;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Manejadores
{
    public class ActionsManager : IDisposable
    {
        private Account account;
        public LuaScriptManager manejador_script;
        private ConcurrentQueue<ScriptAction> fila_acciones;
        public ScriptAction accion_actual;
        private DynValue coroutine_actual;
        private TimerWrapper timer_out;
        public int contador_pelea, contador_recoleccion, contador_peleas_mapa,compteur_capture;
        private bool mapa_cambiado;
        private bool disposed;

        public event Action<bool> evento_accion_normal;
        public event Action<bool> evento_accion_personalizada;

        public ActionsManager(Account _cuenta, LuaScriptManager _manejador_script)
        {
            account = _cuenta;
            manejador_script = _manejador_script;
            fila_acciones = new ConcurrentQueue<ScriptAction>();
            timer_out = new TimerWrapper(60000, time_Out_Callback);
            CharacterClass personaje = account.game.character;
            
            account.game.map.mapRefreshEvent += evento_Mapa_Cambiado;
            account.game.fight.pelea_creada += get_Pelea_Creada;
            account.game.manager.movimientos.movimiento_finalizado += evento_Movimiento_Celda;
            personaje.dialogo_npc_recibido += npcs_Dialogo_Recibido;
            personaje.dialogo_npc_acabado += npcs_Dialogo_Acabado;
            personaje.inventario.almacenamiento_abierto += iniciar_Almacenamiento;
            personaje.inventario.almacenamiento_cerrado += cerrar_Almacenamiento;
            account.game.manager.recoleccion.recoleccion_iniciada += get_Recoleccion_Iniciada;
            account.game.manager.recoleccion.recoleccion_acabada += get_Recoleccion_Acabada;
        }

        private void evento_Mapa_Cambiado()
        {
            if (!account.script.InExecution || accion_actual == null)
                return;

            mapa_cambiado = true;

            // cuando inicia una pelea "resetea el mapa"
            if (!(accion_actual is PeleasAccion))
                contador_peleas_mapa = 0;

            if (accion_actual is ChangeMapAction || accion_actual is PeleasAccion || accion_actual is RecoleccionAccion || coroutine_actual != null)
            {
                limpiar_Acciones();
                NextAction(1500);
            }
        }

        private async void evento_Movimiento_Celda(bool es_correcto)
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is PeleasAccion)
            {
                if (es_correcto)
                {
                    for (int delay = 0; delay < 10000 && account.AccountState != AccountStates.FIGHTING; delay += 500)
                        await Task.Delay(500);

                    if (account.AccountState != AccountStates.FIGHTING)
                    {
                        account.Logger.LogDanger("SCRIPT", "Erreur en lançant le combat, les monstres ont pu se déplacer ou être volés ! Il se peut que vous ayez changé de map en essayant d'attaquer un monstre !");
                        if(account.hasGroup && account.isGroupLeader)
                        {
                            foreach (var member in account.group.members)
                            {
                                try
                                {

                                    if (member.AccountState == AccountStates.CONNECTED_INACTIVE && member.game.map.mapId != account.game.map.mapId)
                                    {
                                        var xOffset = Math.Abs(member.game.map.x) - Math.Abs(account.game.map.x);
                                        var yOffset = Math.Abs(member.game.map.y) - Math.Abs(account.game.map.y);

                                        //Max allowed 1 map
                                        //if leader has moved top or bottom
                                        if ((yOffset == 1 || yOffset == -1) && xOffset == 0)
                                        {
                                            if (yOffset == -1)
                                            {
                                                if (member.game.map.y >= 0 || account.game.map.y >= 0)
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("BOTTOM");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par le bas {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.BOTTOM, cell, true);
                                                }
                                                else
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("TOP");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par le haut {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.TOP, cell, true);
                                                }
                                            }
                                            else
                                            {
                                                if (member.game.map.y >= 0 || account.game.map.y >= 0)
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("TOP");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par le haut {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.TOP, cell, true);
                                                }
                                                else
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("BOTTOM");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par le bas {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.BOTTOM, cell, true);
                                                }
                                            }
                                        }
                                        else if ((xOffset == 1 || xOffset == -1) && yOffset == 0)
                                        {
                                            if (xOffset == -1)
                                            {
                                                if (member.game.map.x >= 0 || account.game.map.x >= 0)
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("RIGHT");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par la droite {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.RIGHT, cell, true);
                                                }
                                                else
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("LEFT");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par la gauche {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.LEFT, cell, true);
                                                }
                                            }
                                            else
                                            {
                                                if (member.game.map.x >= 0 || account.game.map.x >= 0)
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("LEFT");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par la gauche {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.LEFT, cell, true);
                                                }
                                                else
                                                {
                                                    var cellDirection = member.game.map.TransformToCellId("RIGHT");
                                                    var cell = member.game.map.GetCellFromId(short.Parse(cellDirection));
                                                    member.Logger.LogDanger("SCRIPT", $"Je rejoins le leader du groupe en passant par la droite {cell.cellId}");
                                                    member.game.manager.movimientos.get_Cambiar_Mapa(Game.Entidades.Manejadores.Movimientos.MapaTeleportCeldas.RIGHT, cell, true);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            account.Logger.LogDanger("SCRIPT", $"Le leader se trouve à plus de une map ! Impossible de le rejoindre X {xOffset} Y {yOffset}");
                                        }

                                    }
                                    else
                                    {
                                        account.Logger.LogDanger("SCRIPT", "Le membre n'est pas inactif ou est déjà sur la même map que le leader");
                                        if (member.AccountState == AccountStates.FIGHTING && account.AccountState != AccountStates.FIGHTING)
                                            account.AccountState = AccountStates.FIGHTING;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    account.Logger.LogException("SCRIPT", ex);
                                }
                            }
                        }
                        NextAction(2500);
                    }
                }
            }
            else if (accion_actual is MoverCeldaAccion celda)
            {
                if (es_correcto)
                    NextAction(0);
                else
                    account.script.detener_Script("erreur lors du déplacement vers la cellule" + celda.celda_id);
            }
            else if (accion_actual is ChangeMapAction && !es_correcto)
                account.script.detener_Script("erreur lors du changement de carte");
        }

        private void get_Recoleccion_Iniciada()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is RecoleccionAccion)
            {
                contador_recoleccion++;

                if (manejador_script.get_Global_Or("COMPTEUR_RECOLTE", DataType.Boolean, false))
                    account.Logger.LogInfo("SCRIPT", $"RECOLTE #{contador_recoleccion}");
            }
        }

        private void get_Recoleccion_Acabada(RecoleccionResultado resultado)
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is RecoleccionAccion)
            {
                switch (resultado)
                {
                    case RecoleccionResultado.FALLO:
                        account.script.detener_Script("Erreur de récolte");
                    break;

                    default:
                        NextAction(800);
                    break;
                }
            }
        }

        private void get_Pelea_Creada()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is PeleasAccion)
            {
                timer_out.Stop();
                contador_peleas_mapa++;
                contador_pelea++;
                if(account.needToCapture == true)
                    compteur_capture++;
                if (account.hasGroup == true && account.needToCapture == false)
                {
                    foreach (var item in account.group.members)
                    {
                        if(item.needToCapture == true)
                        {
                            compteur_capture++;
                        }
                    }
                }

                if (manejador_script.get_Global_Or("COMPTEUR_COMBAT", DataType.Boolean, false) && compteur_capture ==0)
                    account.Logger.LogInfo("SCRIPT", $"Combat #{contador_pelea}");
                else if (manejador_script.get_Global_Or("COMPTEUR_CAPTURE", DataType.Boolean, false) == true)
                    account.Logger.LogInfo("SCRIPT", $"Combat #{contador_pelea} , Capture  tenté:" + compteur_capture);
            }
        }

        private void npcs_Dialogo_Recibido()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is NpcBankAction nba || (account.hasGroup && account.group.lider.script.actions_manager.accion_actual is NpcBankAction))
            {
                if (account.AccountState != AccountStates.DIALOG)
                    return;

                IEnumerable<Npcs> npcs = account.game.map.lista_npcs();
                Npcs npc = npcs.ElementAt((account.game.character.hablando_npc_id * -1) - 1);
                account.connexion.SendPacket("DR" + npc.pregunta + "|" + npc.respuestas[0], true);
            }
            else if (accion_actual is NpcAction || accion_actual is RespuestaAccion)
                NextAction(400);
        }

        private void npcs_Dialogo_Acabado()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is RespuestaAccion || accion_actual is CerrarVentanaAccion)
                NextAction(200); 
        }

        public void enqueue_Accion(ScriptAction accion, bool iniciar_dequeue_acciones = false)
        {
            fila_acciones.Enqueue(accion);

            if (iniciar_dequeue_acciones)
                NextAction(0);
        }

        public void get_Funcion_Personalizada(DynValue coroutine)
        {
            if (!account.script.InExecution || coroutine_actual != null)
                return;

            coroutine_actual = manejador_script.script.CreateCoroutine(coroutine);
            procesar_Coroutine();
        }

        private void limpiar_Acciones()
        {
            while (fila_acciones.TryDequeue(out ScriptAction temporal)) { };
            accion_actual = null;
        }

        private void iniciar_Almacenamiento()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is NpcBankAction)
                NextAction(400);
        }

        private void cerrar_Almacenamiento()
        {
            if (!account.script.InExecution)
                return;

            if (accion_actual is CerrarVentanaAccion)
                NextAction(400);
        }

        private void procesar_Coroutine()
        {
            if (!account.script.InExecution)
                return;

            try
            {
                DynValue result = coroutine_actual.Coroutine.Resume();

                if (result.Type == DataType.Void)
                    acciones_Funciones_Finalizadas();
            }
            catch (Exception ex)
            {
                account.script.detener_Script(ex.ToString());
            }
        }

        private async Task procesar_Accion_Actual()
        {
            if (!account.script.InExecution)
                return;

            string tipo = accion_actual.GetType().Name;

            switch (await accion_actual.process(account))
            {
                case ResultadosAcciones.HECHO:
                    NextAction(100);
                break;

                case ResultadosAcciones.FALLO:
                    account.Logger.LogDanger("SCRIPT", $"{tipo} failed to process.");
                                       
               break;

                case ResultadosAcciones.PROCESANDO:
                    timer_out.Start();
                break;
            }
        }

        private void time_Out_Callback(object state)
        {
            if (!account.script.InExecution)
                return;

            account.Logger.LogDanger("SCRIPT", "Temps de finition");
            account.script.detener_Script();
            account.script.activar_Script();
        }

        private void acciones_Finalizadas()
        {
            if (mapa_cambiado)
            {
                mapa_cambiado = false;
                evento_accion_normal?.Invoke(true);
            }
            else
                evento_accion_normal?.Invoke(false);
        }

        private void acciones_Funciones_Finalizadas()
        {
            coroutine_actual = null;

            if (mapa_cambiado)
            {
                mapa_cambiado = false;
                evento_accion_personalizada?.Invoke(true);
            }
            else
                evento_accion_personalizada?.Invoke(false);
        }

        private void NextAction(int delay) => Task.Factory.StartNew(async () =>
        {
            if (account?.script.InExecution == false)
                return;

            if (timer_out.isEnabled)
                timer_out.Stop();

            if (delay > 0)
                await Task.Delay(delay);

            if (fila_acciones.Count > 0)
            {
                if (fila_acciones.TryDequeue(out ScriptAction accion))
                {
                    accion_actual = accion;
                    await procesar_Accion_Actual();
                }
            }
            else
            {
                if (coroutine_actual != null)
                    procesar_Coroutine();
                else
                    acciones_Finalizadas();
            }

        }, TaskCreationOptions.LongRunning);

        public void get_Borrar_Todo()
        {
            limpiar_Acciones();
            accion_actual = null;
            coroutine_actual = null;
            timer_out.Stop();

            contador_pelea = 0;
            contador_peleas_mapa = 0;
            contador_recoleccion = 0;
        }

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~ActionsManager() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    timer_out.Dispose();
                }
                accion_actual = null;
                fila_acciones = null;
                account = null;
                manejador_script = null;
                timer_out = null;
                disposed = true;
            }
        }
        #endregion
    }
}
