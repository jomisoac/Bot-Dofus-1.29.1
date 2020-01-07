using Bot_Dofus_1._29._1.Otros.Game.Character.Spells;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilities.Config;
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
    public class SpellsManager : IDisposable
    {
        private Account cuenta;
        private Map mapa;
        private Fight pelea;
        private bool disposed;

        public SpellsManager(Account _cuenta)
        {
            cuenta = _cuenta;
            mapa = cuenta.game.map;
            pelea = cuenta.game.fight;
        }

        public async Task<ResultadoLanzandoHechizo> manejador_Hechizos(HechizoPelea hechizo,bool capturer = false)
        {
            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return await lanzar_Hechizo_Celda_Vacia(hechizo);

            if (hechizo.metodo_lanzamiento == MetodoLanzamiento.AMBOS)
                return await get_Lanzar_Hechizo_Simple(hechizo,capturer);

            if (hechizo.metodo_lanzamiento == MetodoLanzamiento.ALEJADO && !cuenta.game.fight.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Lanzar_Hechizo_Simple(hechizo);

            if (hechizo.metodo_lanzamiento == MetodoLanzamiento.CAC && cuenta.game.fight.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Lanzar_Hechizo_Simple(hechizo);

            if (hechizo.metodo_lanzamiento == MetodoLanzamiento.CAC && !cuenta.game.fight.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Mover_Lanzar_hechizo_Simple(hechizo, get_Objetivo_Mas_Cercano(hechizo));

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> get_Lanzar_Hechizo_Simple(HechizoPelea hechizo,bool capturer= false)
        {
            if (pelea.get_Puede_Lanzar_hechizo(hechizo.id) != FallosLanzandoHechizo.NINGUNO)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Luchadores enemigo = get_Objetivo_Mas_Cercano(hechizo);

            /* gestion sort capture */
            bool lancer_capture = is_Spell_capture_lancable();
            if(cuenta.isGroupLeader== true)
            {
                if (cuenta.needToCapture == true && cuenta.game.fight.jugador_luchador.esta_vivo == false)
                {
                    capturer = false;
                }
            }
            if (cuenta.hasGroup ==true)
            {
                foreach (var membre in cuenta.group.members)
                {
                    if(membre.needToCapture == true && membre.game.fight.jugador_luchador.esta_vivo ==false)
                    {
                        capturer = false;
                    }
                }
            }
                
            if (lancer_capture == true &&  cuenta.needToCapture == false && cuenta.capturelance == false && capturer == true && cuenta.capturefight == true)
            {
                    cuenta.Logger.LogInfo($"Fight", $"CAPTURE INFO : Je lance pas de sort, la capture doit étre lancé par un autre le captureur");
                    return ResultadoLanzandoHechizo.NO_LANZADO;
            }
            if (lancer_capture == true && cuenta.needToCapture == true && capturer == true &&  cuenta.capturelance == false && cuenta.capturefight ==true)
            {
                FallosLanzandoHechizo resultado2 = pelea.get_Puede_Lanzar_hechizo(413, pelea.jugador_luchador.celda, pelea.jugador_luchador.celda, mapa);
                
                if (resultado2 == FallosLanzandoHechizo.NINGUNO)
                {
                    cuenta.Logger.LogInfo($"Fight", $"CAPTURE INFO : Je lance la capture");
                    await pelea.get_Lanzar_Hechizo(413, pelea.jugador_luchador.celda.cellId);
                    
                    if(cuenta.hasGroup == true)
                    {
                        cuenta.group.lider.capturelance = true;
                        foreach (var item in cuenta.group.members)
                        {
                            item.capturelance = true;
                        }
                    }
                    else
                    {
                        cuenta.capturelance = true;
                    }
                    return ResultadoLanzandoHechizo.LANZADO;
                }
                else
                {
                    cuenta.Logger.LogInfo($"Fight", $"CAPTURE INFO : Je doit lancer la capture mais pas possible ( manque PA ) ");
                    return ResultadoLanzandoHechizo.NO_LANZADO;
                }
            }
            else if(lancer_capture == false && hechizo.id == 413 && cuenta.needToCapture == true &&  capturer == true && cuenta.capturelance == false && cuenta.capturefight == true)
            {
                cuenta.Logger.LogInfo($"Fight", $"CAPTURE INFO : Pas le moment pour lancer la capture");
                return ResultadoLanzandoHechizo.NO_LANZADO;
            }
            



            if (enemigo != null)
            {
                FallosLanzandoHechizo resultado = pelea.get_Puede_Lanzar_hechizo(hechizo.id, pelea.jugador_luchador.celda, enemigo.celda, mapa);

                if (resultado == FallosLanzandoHechizo.NINGUNO)
                {
                    await pelea.get_Lanzar_Hechizo(hechizo.id, enemigo.celda.cellId);
                    return ResultadoLanzandoHechizo.LANZADO;
                }

                if (resultado == FallosLanzandoHechizo.NO_ESTA_EN_RANGO)
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

            foreach (KeyValuePair<short, MovimientoNodo> movimiento in PeleasPathfinder.get_Celdas_Accesibles(pelea, mapa, pelea.jugador_luchador.celda))
            {
                if (!movimiento.Value.alcanzable)
                    continue;

                if (hechizo_pelea.metodo_lanzamiento == MetodoLanzamiento.CAC && !pelea.esta_Cuerpo_A_Cuerpo_Con_Aliado(mapa.GetCellFromId(movimiento.Key)))
                    continue;

                if (pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id, mapa.GetCellFromId(movimiento.Key), enemigo.celda, mapa) != FallosLanzandoHechizo.NINGUNO)
                    continue;

                if (movimiento.Value.camino.celdas_accesibles.Count <= pm_utilizados)
                {
                    nodo = movimiento;
                    pm_utilizados = movimiento.Value.camino.celdas_accesibles.Count;
                }
            }

            if (nodo != null)
            {
                await cuenta.game.manager.movimientos.get_Mover_Celda_Pelea(nodo);
                return ResultadoLanzandoHechizo.MOVIDO;
            }

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> lanzar_Hechizo_Celda_Vacia(HechizoPelea hechizo_pelea)
        {
            if (pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id) != FallosLanzandoHechizo.NINGUNO)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            if (hechizo_pelea.focus == HechizoFocus.CELDA_VACIA && pelea.get_Cuerpo_A_Cuerpo_Enemigo().Count() == 4)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Spell hechizo = cuenta.game.character.get_Hechizo(hechizo_pelea.id);
            SpellStats datos_hechizo = hechizo.get_Stats();

            List<short> rangos_disponibles = pelea.get_Rango_hechizo(pelea.jugador_luchador.celda, datos_hechizo, mapa);
            foreach (short rango in rangos_disponibles)
            {
                if (pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id, pelea.jugador_luchador.celda, mapa.GetCellFromId(rango), mapa) == FallosLanzandoHechizo.NINGUNO)
                {
                    if (hechizo_pelea.metodo_lanzamiento == MetodoLanzamiento.CAC || hechizo_pelea.metodo_lanzamiento == MetodoLanzamiento.AMBOS && mapa.GetCellFromId(rango).GetDistanceBetweenCells(pelea.jugador_luchador.celda) != 1)
                        continue;

                    await pelea.get_Lanzar_Hechizo(hechizo_pelea.id, rango);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
            }

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private Luchadores get_Objetivo_Mas_Cercano(HechizoPelea hechizo, short sortId = 0)
        {
         
                Spell Spell = cuenta.game.character.get_Hechizo(hechizo.id);

            if(sortId == 413)
            {
                return pelea.jugador_luchador;
            }

            SpellStats SpellStats = Spell.get_Stats();
            int range = SpellStats.alcanze_maximo;

            if (hechizo.focus == HechizoFocus.ENCIMA)
                return pelea.jugador_luchador;

            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return null;

            return hechizo.focus == HechizoFocus.ENEMIGO ? pelea.get_Obtener_Enemigo_Mas_Cercano(range) : pelea.get_Obtener_Aliado_Mas_Cercano();
        }



        private bool is_Spell_capture_lancable()
        {
            bool retour = false;
            if (pelea.get_Enemigos.Count() == 1)
            {
                Luchadores dernierenemi = pelea.get_Obtener_Enemigo_Mas_Cercano();
                int viePourcentage = (int)dernierenemi.vida_actual / dernierenemi.vida_maxima;

                if (viePourcentage <= 70)
                {
                    retour = true;
                }
            }
            return retour;
        }


        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~SpellsManager() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                cuenta = null;
                mapa = null;
                pelea = null;
                disposed = true;
            }
        }
        #endregion
    }
}
