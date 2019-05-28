using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Manejadores.Movimientos
{
    public class Movimiento : IDisposable
    {
        private Cuenta cuenta;
        private Mapa mapa;
        private Pathfinder pathfinder;
        public List<short> actual_path;

        public event Action<bool> movimiento_finalizado;
        private bool disposed;

        public Movimiento(Cuenta _cuenta, Mapa _mapa)
        {
            cuenta = _cuenta;
            mapa = _mapa;

            pathfinder = new Pathfinder();
            mapa.mapa_actualizado += evento_Mapa_Actualizado;
        }

        public bool get_Puede_Cambiar_Mapa(MapaTeleportCeldas direccion, Celda celda)
        {
            switch (direccion)
            {
                case MapaTeleportCeldas.IZQUIERDA:
                    return (celda.x - 1) == celda.y;

                case MapaTeleportCeldas.DERECHA:
                    return (celda.x - 27) == celda.y;

                case MapaTeleportCeldas.ABAJO:
                    return (celda.x + celda.y) == 31;

                case MapaTeleportCeldas.ARRIBA:
                    return celda.y < 0 && (celda.x - Math.Abs(celda.y)) == 1;
            }

            return true; // direccion NINGUNA
        }

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion, Celda celda, bool esquivar_monstruos)
        {
            if (cuenta.esta_ocupado)
                return false;

            if (!get_Puede_Cambiar_Mapa(direccion, celda))
                return false;

            return get_Mover_Para_Cambiar_mapa(celda, esquivar_monstruos);
        }

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion, bool esquivar_monstruos)
        {
            if (cuenta.esta_ocupado)
                return false;

            List<Celda> celdas_teleport = cuenta.juego.mapa.celdas.Where(celda => celda.tipo == TipoCelda.CELDA_TELEPORT).Select(celda => celda).ToList();

            while (celdas_teleport.Count > 0)
            {
                Celda celda = celdas_teleport[Randomize.get_Random_Int(0, celdas_teleport.Count)];

                if (get_Cambiar_Mapa(direccion, celda, esquivar_monstruos))
                    return true;

                celdas_teleport.Remove(celda);
            }

            cuenta.logger.log_Peligro("MOVIMIENTOS", "No se ha encontrado celda de destino");
            return false;
        }

        public ResultadoMovimientos get_Mover_A_Celda(Celda celda_destino, List<Celda> celdas_no_permitidas, bool detener_delante = false)
        {
            if (celda_destino.id < 0 || celda_destino.id > mapa.celdas.Length)
                return ResultadoMovimientos.FALLO;

            if (cuenta.esta_ocupado || actual_path != null)
                return ResultadoMovimientos.FALLO;
            
            if (celda_destino.id == cuenta.juego.personaje.celda.id)
                return ResultadoMovimientos.MISMA_CELDA;

            if (celda_destino.tipo == TipoCelda.NO_CAMINABLE && celda_destino.objeto_interactivo == null)
                return ResultadoMovimientos.FALLO;

            if (celda_destino.tipo == TipoCelda.OBJETO_INTERACTIVO && celda_destino.objeto_interactivo == null)
                return ResultadoMovimientos.FALLO;

            List<short> path_temporal = pathfinder.get_Path(cuenta.juego.personaje.celda, celda_destino, celdas_no_permitidas);
            
            if (path_temporal == null || path_temporal.Count == 0)
            {
                cuenta.logger.log_Peligro("MOVIMIENTOS", "Pathfinder inválido o vacío");
                return ResultadoMovimientos.PATHFINDING_ERROR;
            }

            actual_path = path_temporal;
            enviar_Paquete_Movimiento();
            return ResultadoMovimientos.EXITO;
        }

        public async Task get_Mover_Celda_Pelea(KeyValuePair<short, MovimientoNodo>? nodo)
        {
            if (!cuenta.esta_luchando())
                return;

            if (nodo == null || nodo.Value.Value.camino.celdas_accesibles.Count == 0)
                return;

            if (nodo.Value.Key == cuenta.pelea.jugador_luchador.celda_id)
                return;

            nodo.Value.Value.camino.celdas_accesibles.Insert(0, cuenta.pelea.jugador_luchador.celda_id);

            await cuenta.conexion.enviar_Paquete_Async("GA001" + PathFinderUtil.get_Pathfinding_Limpio(nodo.Value.Value.camino.celdas_accesibles, true, mapa));
            cuenta.juego.personaje.evento_Personaje_Pathfinding_Minimapa(nodo.Value.Value.camino.celdas_accesibles);
        }

        private bool get_Mover_Para_Cambiar_mapa(Celda celda, bool esquivar_monstruos)
        {
            switch (get_Mover_A_Celda(celda, mapa.celdas_ocupadas))
            {
                case ResultadoMovimientos.EXITO:
                    if (GlobalConf.mostrar_mensajes_debug)
                        cuenta.logger.log_informacion("MOVIMIENTOS", $"Mapa actual: {mapa.id} desplazando para cambiar el mapa a la casilla: " + celda.id);
                return true;

                default:
                    if (GlobalConf.mostrar_mensajes_debug)
                        cuenta.logger.log_informacion("MOVIMIENTOS", $"camino hacia {celda.id} fallado o bloqueado");
                return false;
            }
        }

        private void enviar_Paquete_Movimiento()
        {
            string path_string = PathFinderUtil.get_Pathfinding_Limpio(actual_path, false, mapa);
            cuenta.conexion.enviar_Paquete("GA001" + path_string);
            cuenta.juego.personaje.evento_Personaje_Pathfinding_Minimapa(actual_path);
        }

        public async Task evento_Movimiento_Finalizado(Celda celda_destino, byte tipo_gkk, bool correcto)
        {
            cuenta.Estado_Cuenta = EstadoCuenta.MOVIMIENTO;

            if (correcto)
            {
                await Task.Delay(PathFinderUtil.get_Tiempo_Desplazamiento_Mapa(cuenta.juego.personaje.celda.id, actual_path, mapa));

                //por si en el delay el bot esta desconectado
                if (cuenta == null || cuenta.Estado_Cuenta == EstadoCuenta.DESCONECTADO)
                    return;

                cuenta.conexion.enviar_Paquete("GKK" + tipo_gkk);
                cuenta.juego.personaje.celda = celda_destino;
            }

            actual_path = null;
            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
            movimiento_finalizado?.Invoke(correcto);
        }

        public void limpiar()
        {
            actual_path = null;
        }

        private void evento_Mapa_Actualizado() => pathfinder.set_Mapa(cuenta.juego.mapa);

        #region Zona Dispose
        ~Movimiento() => Dispose(false);
        public void Dispose() => Dispose(true);

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

                disposed = true;
            }
        }
        #endregion
    }
}
