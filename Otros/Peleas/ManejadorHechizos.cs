using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
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

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class ManejadorHechizos : IDisposable
    {
        private Cuenta cuenta;
        private bool disposed;

        public ManejadorHechizos(Cuenta _cuenta) => cuenta = _cuenta;

        public async Task<ResultadoLanzandoHechizo> manejador_Hechizos(HechizoPelea hechizo)
        {
            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return await lanzar_Hechizo_Celda_Vacia(hechizo);

            if (!hechizo.lanzar_cuerpo_cuerpo && !hechizo.es_aoe)
                return await get_Lanzar_Hechizo_Simple(hechizo);

            //si el hechizo es cuerpo a cuerpo y esta cuerpo a cuerpo lanzara el hechizo
            if (hechizo.lanzar_cuerpo_cuerpo && !hechizo.es_aoe && cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Lanzar_Hechizo_Simple(hechizo);

            //si el hechizo es cuerpo a cuerpo pero no esta cuerpo a cuerpo hay que mover el bot
            if (hechizo.lanzar_cuerpo_cuerpo && !hechizo.es_aoe && !cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Mover_Lanzar_hechizo_Simple(hechizo, get_Objetivo_Mas_Cercano(hechizo));

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> get_Lanzar_Hechizo_Simple(HechizoPelea hechizo)
        {
            Pelea pelea = cuenta.pelea;
            FallosLanzandoHechizo resultado_puede_lanzar_hechizo = pelea.get_Puede_Lanzar_hechizo(hechizo.id);

            if (resultado_puede_lanzar_hechizo != FallosLanzandoHechizo.NINGUNO)
            {
                if (GlobalConf.mostrar_mensajes_debug)
                    cuenta.logger.log_informacion("DEBUG", $"Hechizo {hechizo.nombre} no lanzado motivo: " + resultado_puede_lanzar_hechizo);
                return ResultadoLanzandoHechizo.NO_LANZADO;
            }

            Luchadores enemigo = get_Objetivo_Mas_Cercano(hechizo);

            if (enemigo != null)
            {
                FallosLanzandoHechizo resultado = pelea.get_Puede_Lanzar_hechizo(hechizo.id, pelea.jugador_luchador.celda, enemigo.celda, cuenta.juego.mapa);
                
                if (resultado == FallosLanzandoHechizo.NINGUNO)
                {
                    await pelea.get_Lanzar_Hechizo(hechizo.id, enemigo.celda.id);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
                else if (resultado == FallosLanzandoHechizo.NO_ESTA_EN_RANGO)
                    return await get_Mover_Lanzar_hechizo_Simple(hechizo, enemigo);
            }
            else if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return await lanzar_Hechizo_Celda_Vacia(hechizo);

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> get_Mover_Lanzar_hechizo_Simple(HechizoPelea hechizo_pelea, Luchadores enemigo)
        {
            KeyValuePair<short, MovimientoNodo>? nodo = null;
            int pm_utilizados = 99;
            Mapa mapa = cuenta.juego.mapa;

            foreach (KeyValuePair<short, MovimientoNodo> movimiento in PeleasPathfinder.get_Celdas_Accesibles(cuenta.pelea, cuenta.juego.mapa, cuenta.pelea.jugador_luchador.celda))
            {
                if (!movimiento.Value.alcanzable)
                    continue;

                if (hechizo_pelea.lanzar_cuerpo_cuerpo && !cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Aliado(mapa.get_Celda_Id(movimiento.Key)))
                    continue;

                if (cuenta.pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id, mapa.get_Celda_Id(movimiento.Key), enemigo.celda, mapa) != FallosLanzandoHechizo.NINGUNO)
                    continue;

                if (movimiento.Value.camino.celdas_accesibles.Count <= pm_utilizados)
                {
                    nodo = movimiento;
                    pm_utilizados = movimiento.Value.camino.celdas_accesibles.Count;
                }
            }

            if (nodo != null)
            {
                await cuenta.juego.manejador.movimientos.get_Mover_Celda_Pelea(nodo);
                return ResultadoLanzandoHechizo.MOVIDO;
            }

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> lanzar_Hechizo_Celda_Vacia(HechizoPelea hechizo_pelea)
        {
            FallosLanzandoHechizo resultado_puede_lanzar_hechizo = cuenta.pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id);

            if (resultado_puede_lanzar_hechizo != FallosLanzandoHechizo.NINGUNO)
            {
                if (GlobalConf.mostrar_mensajes_debug)
                    cuenta.logger.log_informacion("DEBUG", $"Hechizo {hechizo_pelea.nombre} no lanzado motivo: " + resultado_puede_lanzar_hechizo);
                return ResultadoLanzandoHechizo.NO_LANZADO;
            }

            if (hechizo_pelea.focus == HechizoFocus.CELDA_VACIA && cuenta.pelea.get_Cuerpo_A_Cuerpo_Enemigo().Count() == 4)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Hechizo hechizo = cuenta.juego.personaje.get_Hechizo(hechizo_pelea.id);
            HechizoStats datos_hechizo = hechizo.get_Stats();
            Mapa mapa = cuenta.juego.mapa;

            List<int> rangos_disponibles = cuenta.pelea.get_Rango_hechizo(cuenta.pelea.jugador_luchador.celda, datos_hechizo, mapa);
            foreach (short rango in rangos_disponibles)
            {
                if (cuenta.pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id, cuenta.pelea.jugador_luchador.celda, mapa.get_Celda_Id(rango), mapa) == FallosLanzandoHechizo.NINGUNO)
                {
                    if (hechizo_pelea.lanzar_cuerpo_cuerpo && mapa.get_Celda_Id(rango).get_Distancia_Entre_Dos_Casillas(cuenta.pelea.jugador_luchador.celda) != 1)
                        continue;

                    await cuenta.pelea.get_Lanzar_Hechizo(hechizo_pelea.id, rango);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
            }

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private Luchadores get_Objetivo_Mas_Cercano(HechizoPelea hechizo)
        {
            if (hechizo.focus == HechizoFocus.ENCIMA)
                return cuenta.pelea.jugador_luchador;

            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return null;

            return hechizo.focus == HechizoFocus.ENEMIGO ? cuenta.pelea.get_Obtener_Enemigo_Mas_Cercano() : cuenta.pelea.get_Obtener_Aliado_Mas_Cercano();
        }

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~ManejadorHechizos() => Dispose(false);
        
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
