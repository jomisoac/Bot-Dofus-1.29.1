using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos
{
    public class Movimiento : IDisposable
    {
        private Account cuenta;
        private CharacterClass personaje;
        private Map mapa;
        private Pathfinder pathfinder;
        public List<Cell> actual_path;

        public event Action<bool> movimiento_finalizado;
        private bool disposed;

        public Movimiento(Account _cuenta, Map _mapa, CharacterClass _personaje)
        {
            cuenta = _cuenta;
            personaje = _personaje;
            mapa = _mapa;

            pathfinder = new Pathfinder();
            mapa.mapRefreshEvent += evento_Mapa_Actualizado;
        }

        public bool get_Puede_Cambiar_Mapa(MapaTeleportCeldas direccion, Cell celda)
        {
            switch (direccion)
            {
                case MapaTeleportCeldas.LEFT:
                    return (celda.x - 1) == celda.y;

                case MapaTeleportCeldas.RIGHT:
                    return (celda.x - 27) == celda.y;

                case MapaTeleportCeldas.BOTTOM:
                    return (celda.x + celda.y) == 31;

                case MapaTeleportCeldas.TOP:
                    return celda.y < 0 && (celda.x - Math.Abs(celda.y)) == 1;
            }

            return true; // direccion NINGUNA
        }

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion, Cell celda, bool ignoreGroupOnSun = false)
        {
            if (cuenta.Is_Busy() || personaje.inventario.porcentaje_pods >= 100)
                return false;

            if (!get_Puede_Cambiar_Mapa(direccion, celda))
                return false;

            return get_Mover_Para_Cambiar_mapa(celda, ignoreGroupOnSun);
        }

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion)
        {
            if (cuenta.Is_Busy())
                return false;

            List<Cell> celdas_teleport = cuenta.game.map.mapCells.Where(celda => celda.cellType == CellTypes.TELEPORT_CELL).Select(celda => celda).ToList();

            while (celdas_teleport.Count > 0)
            {
                Cell celda = celdas_teleport[Randomize.get_Random(0, celdas_teleport.Count)];

                if (get_Cambiar_Mapa(direccion, celda))
                    return true;

                celdas_teleport.Remove(celda);
            }

            cuenta.Logger.LogDanger("MOUVEMENT", "Aucune cellule de destination trouvée, utiliser la méthode : TOP|BOTTOM|RIGHT|LEFT");
            return false;
        }

        public ResultadoMovimientos get_Mover_A_Celda(Cell celda_destino, List<Cell> celdas_no_permitidas, bool detener_delante = false, byte distancia_detener = 0)
        {
            if (celda_destino.cellId < 0 || celda_destino.cellId > mapa.mapCells.Length)
                return ResultadoMovimientos.CellRangeError;

            if (cuenta.Is_Busy() || personaje.inventario.porcentaje_pods >= 100)
                return ResultadoMovimientos.CharacterBusyOrFull;

            if (celda_destino.cellId == personaje.celda.cellId)
                return ResultadoMovimientos.SameCell;

            if (celda_destino.cellType == CellTypes.NOT_WALKABLE && celda_destino.interactiveObject == null)
                return ResultadoMovimientos.CellNotWalkable;

            if (celda_destino.cellType == CellTypes.INTERACTIVE_OBJECT && celda_destino.interactiveObject == null)
                return ResultadoMovimientos.CellIsTypeOfInteractiveObject;

            if (celdas_no_permitidas.Contains(celda_destino))
                return ResultadoMovimientos.MonsterOnSun;

            List<Cell> path_temporal = pathfinder.get_Path(personaje.celda, celda_destino, celdas_no_permitidas, detener_delante, distancia_detener);

            if (path_temporal == null || path_temporal.Count == 0)
                return ResultadoMovimientos.PathfindingError;

            if (!detener_delante && path_temporal.Last().cellId != celda_destino.cellId)
                return ResultadoMovimientos.PathfindingError;

            if (detener_delante && path_temporal.Count == 1 && path_temporal[0].cellId == personaje.celda.cellId)
                return ResultadoMovimientos.SameCell;
            
            if (detener_delante && path_temporal.Count == 2 && path_temporal[0].cellId == personaje.celda.cellId && path_temporal[1].cellId == celda_destino.cellId)
                return ResultadoMovimientos.SameCell;

            actual_path = path_temporal;
            enviar_Paquete_Movimiento();
            return ResultadoMovimientos.EXITO;
        }

        public async Task get_Mover_Celda_Pelea(KeyValuePair<short, MovimientoNodo>? nodo)
        {
            if (!cuenta.IsFighting())
                return;

            if (nodo == null || nodo.Value.Value.camino.celdas_accesibles.Count == 0)
                return;

            if (nodo.Value.Key == cuenta.game.fight.jugador_luchador.celda.cellId)
                return;

            nodo.Value.Value.camino.celdas_accesibles.Insert(0, cuenta.game.fight.jugador_luchador.celda.cellId);
            List<Cell> lista_celdas = nodo.Value.Value.camino.celdas_accesibles.Select(c => mapa.GetCellFromId(c)).ToList();
            await cuenta.connexion.SendPacketAsync("GA001" + PathFinderUtil.get_Pathfinding_Limpio(lista_celdas), false);
            personaje.evento_Personaje_Pathfinding_Minimapa(lista_celdas);
        }

        private bool get_Mover_Para_Cambiar_mapa(Cell celda, bool ignoreGroupOnSun = false)
        {
            var cellsNotPermitted = mapa.celdas_ocupadas().Where(c => c.cellType != CellTypes.TELEPORT_CELL).ToList();
            if (ignoreGroupOnSun)
                cellsNotPermitted = new List<Cell>();
            var t = new Random().Next(650, 1500);
            Task.Delay(t);

            ResultadoMovimientos resultado = get_Mover_A_Celda(celda, cellsNotPermitted);
            switch (resultado)
            {
                case ResultadoMovimientos.EXITO:
                        cuenta.Logger.LogInfo("MOUVEMENT", $"{mapa.GetCoordinates} changement de map via la cellule {celda.cellId} ");
                return true;

                default:
                        cuenta.Logger.LogError("MOUVEMENT", $"Chemin vers {celda.cellId} résultat échoué ou bloqué : {resultado}");
                return false;
            }
        }

        private void enviar_Paquete_Movimiento()
        {
            if (cuenta.AccountState == AccountStates.REGENERATION)
                cuenta.connexion.SendPacket("eU1", true);

            string path_string = PathFinderUtil.get_Pathfinding_Limpio(actual_path);
            cuenta.connexion.SendPacket("GA001" + path_string, true);
            personaje.evento_Personaje_Pathfinding_Minimapa(actual_path);
        }

        public async Task evento_Movimiento_Finalizado(Cell celda_destino, byte tipo_gkk, bool correcto)
        {
            cuenta.AccountState = AccountStates.MOVING;

            if (correcto)
            {
                await Task.Delay(PathFinderUtil.get_Tiempo_Desplazamiento_Mapa(personaje.celda, actual_path, personaje.esta_utilizando_dragopavo));

                //por si en el delay el bot esta desconectado
                if (cuenta == null || cuenta.AccountState == AccountStates.DISCONNECTED)
                    return;

                cuenta.connexion.SendPacket("GKK" + tipo_gkk);
                personaje.celda = celda_destino;
            }

            actual_path = null;
            cuenta.AccountState = AccountStates.CONNECTED_INACTIVE;
            movimiento_finalizado?.Invoke(correcto);
        }

        private void evento_Mapa_Actualizado() => pathfinder.set_Mapa(cuenta.game.map);
        public void movimiento_Actualizado(bool estado) => movimiento_finalizado?.Invoke(estado);

        #region Zona Dispose
        ~Movimiento() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void Clear()
        {
            actual_path = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pathfinder.Dispose();
                }

                actual_path?.Clear();
                actual_path = null;
                pathfinder = null;
                cuenta = null;
                personaje = null;
                disposed = true;
            }
        }
        #endregion
    }
}
