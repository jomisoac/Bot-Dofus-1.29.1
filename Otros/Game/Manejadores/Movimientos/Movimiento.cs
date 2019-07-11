using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Personaje;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
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
        private Cuenta cuenta;
        private PersonajeJuego personaje;
        private Mapa mapa;
        private Pathfinder pathfinder;
        public List<Celda> actual_path;

        public event Action<bool> movimiento_finalizado;
        private bool disposed;

        public Movimiento(Cuenta _cuenta, Mapa _mapa, PersonajeJuego _personaje)
        {
            cuenta = _cuenta;
            personaje = _personaje;
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

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion, Celda celda)
        {
            if (cuenta.esta_ocupado() || personaje.inventario.porcentaje_pods >= 100)
                return false;

            if (!get_Puede_Cambiar_Mapa(direccion, celda))
                return false;

            return get_Mover_Para_Cambiar_mapa(celda);
        }

        public bool get_Cambiar_Mapa(MapaTeleportCeldas direccion)
        {
            if (cuenta.esta_ocupado())
                return false;

            List<Celda> celdas_teleport = cuenta.juego.mapa.celdas.Where(celda => celda.tipo == TipoCelda.CELDA_TELEPORT).Select(celda => celda).ToList();

            while (celdas_teleport.Count > 0)
            {
                Celda celda = celdas_teleport[Randomize.get_Random(0, celdas_teleport.Count)];

                if (get_Cambiar_Mapa(direccion, celda))
                    return true;

                celdas_teleport.Remove(celda);
            }

            cuenta.logger.log_Peligro("MOVIMIENTOS", "No se ha encontrado celda de destino, usa el metodo por id");
            return false;
        }

        public ResultadoMovimientos get_Mover_A_Celda(Celda celda_destino, List<Celda> celdas_no_permitidas, bool detener_delante = false, byte distancia_detener = 0)
        {
            if (celda_destino.id < 0 || celda_destino.id > mapa.celdas.Length)
                return ResultadoMovimientos.FALLO;

            if (cuenta.esta_ocupado() || actual_path != null || personaje.inventario.porcentaje_pods >= 100)
                return ResultadoMovimientos.FALLO;

            if (celda_destino.id == personaje.celda.id)
                return ResultadoMovimientos.MISMA_CELDA;

            if (celda_destino.tipo == TipoCelda.NO_CAMINABLE && celda_destino.objeto_interactivo == null)
                return ResultadoMovimientos.FALLO;

            if (celda_destino.tipo == TipoCelda.OBJETO_INTERACTIVO && celda_destino.objeto_interactivo == null)
                return ResultadoMovimientos.FALLO;

            List<Celda> path_temporal = pathfinder.get_Path(personaje.celda, celda_destino, celdas_no_permitidas, detener_delante, distancia_detener);

            if (path_temporal == null || path_temporal.Count == 0)
                return ResultadoMovimientos.PATHFINDING_ERROR;

            if (!detener_delante && path_temporal.Last().id != celda_destino.id)
                return ResultadoMovimientos.PATHFINDING_ERROR;

            if (detener_delante && path_temporal.Count == 1 && path_temporal[0].id == personaje.celda.id)
                return ResultadoMovimientos.MISMA_CELDA;
            
            if (detener_delante && path_temporal.Count == 2 && path_temporal[0].id == personaje.celda.id && path_temporal[1].id == celda_destino.id)
                return ResultadoMovimientos.MISMA_CELDA;

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

            if (nodo.Value.Key == cuenta.juego.pelea.jugador_luchador.celda.id)
                return;

            nodo.Value.Value.camino.celdas_accesibles.Insert(0, cuenta.juego.pelea.jugador_luchador.celda.id);
            List<Celda> lista_celdas = nodo.Value.Value.camino.celdas_accesibles.Select(c => mapa.get_Celda_Id(c)).ToList();

            await cuenta.conexion.enviar_Paquete_Async("GA001" + PathFinderUtil.get_Pathfinding_Limpio(lista_celdas), false);
            personaje.evento_Personaje_Pathfinding_Minimapa(lista_celdas);
        }

        private bool get_Mover_Para_Cambiar_mapa(Celda celda)
        {
            ResultadoMovimientos resultado = get_Mover_A_Celda(celda, mapa.celdas_ocupadas());
            switch (resultado)
            {
                case ResultadoMovimientos.EXITO:
                        cuenta.logger.log_informacion("MOVIMIENTOS", $"Mapa actual: {mapa.id} desplazando para cambiar el mapa a la casilla: {celda.id}");
                return true;

                default:
                        cuenta.logger.log_Error("MOVIMIENTOS", $"camino hacia {celda.id} fallado o bloqueado resultado: {resultado}");
                return false;
            }
        }

        private void enviar_Paquete_Movimiento()
        {
            if (cuenta.Estado_Cuenta == EstadoCuenta.REGENERANDO)
                cuenta.conexion.enviar_Paquete("eU1", true);

            string path_string = PathFinderUtil.get_Pathfinding_Limpio(actual_path);
            cuenta.conexion.enviar_Paquete("GA001" + path_string, true);
            personaje.evento_Personaje_Pathfinding_Minimapa(actual_path);
        }

        public async Task evento_Movimiento_Finalizado(Celda celda_destino, byte tipo_gkk, bool correcto)
        {
            cuenta.Estado_Cuenta = EstadoCuenta.MOVIMIENTO;

            if (correcto)
            {
                await Task.Delay(PathFinderUtil.get_Tiempo_Desplazamiento_Mapa(personaje.celda, actual_path, personaje.esta_utilizando_dragopavo));

                //por si en el delay el bot esta desconectado
                if (cuenta == null || cuenta.Estado_Cuenta == EstadoCuenta.DESCONECTADO)
                    return;

                cuenta.conexion.enviar_Paquete("GKK" + tipo_gkk);
                personaje.celda = celda_destino;
            }

            actual_path = null;
            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
            movimiento_finalizado?.Invoke(correcto);
        }

        private void evento_Mapa_Actualizado() => pathfinder.set_Mapa(cuenta.juego.mapa);
        public void movimiento_Actualizado(bool estado) => movimiento_finalizado?.Invoke(estado);

        #region Zona Dispose
        ~Movimiento() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void limpiar()
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
