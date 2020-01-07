using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Character.Spells;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class Fight : IEliminable, IDisposable
    {
        public Account account { get; private set; }
        private ConcurrentDictionary<int, Luchadores> luchadores;
        private ConcurrentDictionary<int, Luchadores> enemigos;
        private ConcurrentDictionary<int, Luchadores> aliados;
        private Dictionary<int, int> hechizos_intervalo;// hechizoID, turnos intervalo
        private Dictionary<int, int> total_hechizos_lanzados;//hechizoID, total veces
        private Dictionary<int, Dictionary<int, int>> total_hechizos_lanzados_en_celda;//hechizoID (celda, veces)
        public List<Cell> celdas_preparacion;
        public LuchadorPersonaje jugador_luchador { get; private set; }
        public byte estado_pelea;// inicio = 1, posicion = 2, combate = 3, finalizado = 4
        private bool disposed;
        public bool captureLance { get; set; } = false;

        public IEnumerable<Luchadores> get_Aliados => aliados.Values.Where(a => a.esta_vivo);
        public IEnumerable<Luchadores> get_Enemigos => enemigos.Values.Where(e => e.esta_vivo);
        public IEnumerable<Luchadores> get_Luchadores => luchadores.Values.Where(f => f.esta_vivo);
        public int total_enemigos_vivos => get_Enemigos.Count(f => f.esta_vivo);
        public int total_ally_alive => get_Aliados.Count(f => f.esta_vivo);
        public int contador_invocaciones => get_Luchadores.Count(f => f.id_invocador == jugador_luchador.id);
        public List<short> get_Celdas_Ocupadas => get_Luchadores.Select(f => f.celda.cellId).ToList();

        public event Action pelea_creada;
        public event Action fightFinished;
        public event Action turno_iniciado;

        //acciones pelea
        public event Action<short, bool> hechizo_lanzado;
        public event Action<bool> movimiento;

        public Fight(Account _cuenta)
        {
            account = _cuenta;
            luchadores = new ConcurrentDictionary<int, Luchadores>();
            enemigos = new ConcurrentDictionary<int, Luchadores>();
            aliados = new ConcurrentDictionary<int, Luchadores>();
            hechizos_intervalo = new Dictionary<int, int>();
            total_hechizos_lanzados = new Dictionary<int, int>();
            total_hechizos_lanzados_en_celda = new Dictionary<int, Dictionary<int, int>>();
            celdas_preparacion = new List<Cell>();
            estado_pelea = 0;
        }

        public async void Block_OnlyForgroupe()
        {
            await Task.Delay(560);
            account.connexion.SendPacket("fP");
            account.Logger.LogInfo($"Fight", $"Combat autorisé uniquement au groupe");
        }

        public async Task get_Lanzar_Hechizo(short hechizo_id, short celda_id)
        {
            Spell hechizo = account.game.character.get_Hechizo(hechizo_id);

            if (account.AccountState != AccountStates.FIGHTING)
                return;
            var t = new Random().Next(500, 900);
            if(hechizo_id == 413)
                account.Logger.LogInfo($"Fight", $"Attente de : {t} ms pour la CAPTURE");
            else
                  account.Logger.LogInfo($"Fight", $"Attente de : {t} ms pour lancer sort : " + hechizo.nombre);
            await Task.Delay(t);
            await account.connexion.SendPacketAsync("GA300" + hechizo_id + ';' + celda_id, false);
        }

        public void actualizar_Hechizo_Exito(short celda_id, short hechizo_id)
        {
            Spell hechizo = account.game.character.get_Hechizo(hechizo_id);
            if (hechizo == null)
                return;
            SpellStats datos_hechizo = hechizo.get_Stats();

            if (datos_hechizo.intervalo > 0 && !hechizos_intervalo.ContainsKey(hechizo.id))
                hechizos_intervalo.Add(hechizo.id, datos_hechizo.intervalo);

            if (!total_hechizos_lanzados.ContainsKey(hechizo.id))
                total_hechizos_lanzados.Add(hechizo.id, 0);

            total_hechizos_lanzados[hechizo.id]++;

            if (total_hechizos_lanzados_en_celda.ContainsKey(hechizo.id))
            {
                if (!total_hechizos_lanzados_en_celda[hechizo.id].ContainsKey(celda_id))
                    total_hechizos_lanzados_en_celda[hechizo.id].Add(celda_id, 0);

                total_hechizos_lanzados_en_celda[hechizo.id][celda_id]++;
            }
            else
            {
                total_hechizos_lanzados_en_celda.Add(hechizo.id, new Dictionary<int, int>()
                {
                    { celda_id, 1 }
                });
            }
        }

        public void get_Final_Turno(int id_personaje)
        {
            Luchadores luchador = get_Luchador_Por_Id(id_personaje);

            if (luchador == jugador_luchador)
            {
                total_hechizos_lanzados.Clear();
                total_hechizos_lanzados_en_celda.Clear();

                for (int i = hechizos_intervalo.Count - 1; i >= 0; i--)
                {
                    int key = hechizos_intervalo.ElementAt(i).Key;
                    hechizos_intervalo[key]--;
                    if (hechizos_intervalo[key] == 0)
                        hechizos_intervalo.Remove(key);
                }
            }
        }

        public Luchadores get_Luchador_Por_Id(int id)
        {
            if (jugador_luchador != null && jugador_luchador.id == id)
                return jugador_luchador;

            else if (luchadores.TryGetValue(id, out Luchadores luchador))
                return luchador;

            return null;
        }


        public Luchadores get_Enemigo_Mas_Debil()
        {
            int vida = -1;
            Luchadores enemigo = null;

            foreach (Luchadores enemigo_actual in get_Enemigos)
            {
                if (!enemigo_actual.esta_vivo)
                    continue;

                if (vida == -1 || enemigo_actual.porcentaje_vida < vida)
                {
                    vida = enemigo_actual.porcentaje_vida;
                    enemigo = enemigo_actual;
                }
            }
            return enemigo;
        }

        public Luchadores get_Obtener_Aliado_Mas_Cercano()
        {
            int distancia = -1, distancia_temporal;
            Luchadores aliado = null;

            foreach (Luchadores luchador_aliado in get_Aliados)
            {
                if (!luchador_aliado.esta_vivo)
                    continue;

                distancia_temporal = jugador_luchador.celda.GetDistanceBetweenCells(luchador_aliado.celda);

                if (distancia == -1 || distancia_temporal < distancia)
                {
                    distancia = distancia_temporal;
                    aliado = luchador_aliado;
                }
            }

            return aliado;
        }

        public Luchadores get_Obtener_Enemigo_Mas_Cercano(int range = 0)
        {
            int distancia = -1, distancia_temporal;
            bool mobFind = false;
            Luchadores enemigo = null;
            int vieMob = 999999;
            List<KeyValuePair<Luchadores, int>> enemiRange = new List<KeyValuePair<Luchadores, int>>();
            foreach (Luchadores luchador_enemigo in get_Enemigos)
            {
                if (!luchador_enemigo.esta_vivo)
                    continue;

                distancia_temporal = jugador_luchador.celda.GetDistanceBetweenCells(luchador_enemigo.celda);

                if ((distancia == -1 || distancia_temporal < distancia))
                {
                    distancia = distancia_temporal;
                    enemigo = luchador_enemigo;
                }

                if (range != 0)
                {
                    KeyValuePair<Luchadores, int> actual = new KeyValuePair<Luchadores, int>(luchador_enemigo, distancia_temporal);
                    enemiRange.Add(actual);
                }
            }
            if(range != 0 && distancia!=1)
            {
                foreach (var item in enemiRange)
                {
                    if (item.Value <= range)
                    {
                        if (vieMob > item.Key.vida_actual && item.Key.id_invocador !=10) /* focus low hp + non invoc */
                        {
                            vieMob = item.Key.vida_actual;
                            enemigo = item.Key;
                            mobFind = true;
                        }
                    }
                }
            }
            if (range != 0 && distancia != 1 && mobFind == false)
            {
                foreach (var item in enemiRange)
                {
                    if (item.Value <= range)
                    {
                        if (vieMob > item.Key.vida_actual) /* focus low hp */
                        {
                            vieMob = item.Key.vida_actual;
                            enemigo = item.Key;
                            mobFind = true;
                        }
                    }
                }
            }
            return enemigo;
        }

        public void get_Agregar_Luchador(Luchadores luchador)
        {
            if (luchador.id == account.game.character.id)
                jugador_luchador = new LuchadorPersonaje(account.game.character.nombre, account.game.character.nivel, luchador);

            else if (!luchadores.TryAdd(luchador.id, luchador))
                luchador.get_Actualizar_Luchador(luchador.id, luchador.esta_vivo, luchador.vida_actual, luchador.pa, luchador.pm, luchador.celda, luchador.vida_maxima, luchador.equipo, luchador.id_invocador);

            get_Ordenar_Luchadores();
        }

        private void get_Ordenar_Luchadores()
        {
            if (jugador_luchador == null)
                return;

            foreach (Luchadores luchador in get_Luchadores)
            {
                if (aliados.ContainsKey(luchador.id) || enemigos.ContainsKey(luchador.id))
                    continue;

                if (luchador.equipo == jugador_luchador.equipo)
                    aliados.TryAdd(luchador.id, luchador);
                else
                    enemigos.TryAdd(luchador.id, luchador);
            }
        }

        public short get_Celda_Mas_Cercana_O_Lejana(bool cercana, IEnumerable<Cell> celdas_posibles)
        {
            short celda_id = -1;
            int distancia_total = -1;

            foreach (Cell celda_actual in celdas_posibles)
            {
                int temporal_total_distancia = get_Distancia_Desde_Enemigo(celda_actual);

                if (celda_id == -1 || ((cercana && temporal_total_distancia < distancia_total) || (!cercana && temporal_total_distancia > distancia_total)))
                {
                    celda_id = celda_actual.cellId;
                    distancia_total = temporal_total_distancia;
                }
            }

            return celda_id;
        }

        public int get_Distancia_Desde_Enemigo(Cell celda_actual) => get_Enemigos.Sum(e => celda_actual.GetDistanceBetweenCells(e.celda) - 1);

        public Luchadores get_Luchador_Esta_En_Celda(int celda_id)
        {
            if (jugador_luchador?.celda.cellId == celda_id)
                return jugador_luchador;

            return get_Luchadores.FirstOrDefault(f => f.esta_vivo && f.celda.cellId == celda_id);
        }

        public bool es_Celda_Libre(Cell celda) => get_Luchador_Esta_En_Celda(celda.cellId) == null;
        public IEnumerable<Luchadores> get_Cuerpo_A_Cuerpo_Enemigo(Cell celda = null) => get_Enemigos.Where(enemigo => enemigo.esta_vivo && (celda == null ? jugador_luchador.celda.GetDistanceBetweenCells(enemigo.celda) : enemigo.celda.GetDistanceBetweenCells(celda)) == 1);
        public IEnumerable<Luchadores> get_Cuerpo_A_Cuerpo_Aliado(Cell celda = null) => get_Aliados.Where(aliado => aliado.esta_vivo && (celda == null ? jugador_luchador.celda.GetDistanceBetweenCells(aliado.celda) : aliado.celda.GetDistanceBetweenCells(celda)) == 1);
        public IEnumerable<Luchadores> get_Enemi_inferieur_7(Cell celda = null) => get_Enemigos.Where(enemigo => enemigo.esta_vivo && (celda == null ? jugador_luchador.celda.GetDistanceBetweenCells(enemigo.celda) : enemigo.celda.GetDistanceBetweenCells(celda)) < 9);

        public IEnumerable<Luchadores> get_Enemi_superieur_8(Cell celda = null) => get_Enemigos.Where(enemigo => enemigo.esta_vivo && (celda == null ? jugador_luchador.celda.GetDistanceBetweenCells(enemigo.celda) : enemigo.celda.GetDistanceBetweenCells(celda)) > 12);

        public bool esta_Cuerpo_A_Cuerpo_Con_Enemigo(Cell celda = null) => get_Cuerpo_A_Cuerpo_Enemigo(celda).Count() > 0;
        public bool esta_Cuerpo_A_Cuerpo_Con_Aliado(Cell celda = null) => get_Cuerpo_A_Cuerpo_Aliado(celda).Count() > 0;

        public bool is_proche_7(Cell celda = null)
        {
            if (get_Enemi_inferieur_7().Count() > 0)
                return true;
            else
                return false;
        }

        public bool is_loin_8(Cell celda = null)
        {
            if (get_Enemi_superieur_8().Count() > 0)
                return true;
            else
                return false;
        }

        public FallosLanzandoHechizo get_Puede_Lanzar_hechizo(short hechizo_id)
        {
            Spell hechizo = account.game.character.get_Hechizo(hechizo_id);

            if (hechizo == null)
                return FallosLanzandoHechizo.DESONOCIDO;

            SpellStats datos_hechizo = hechizo.get_Stats();

            if (jugador_luchador.pa < datos_hechizo.coste_pa)
                return FallosLanzandoHechizo.PUNTOS_ACCION;

            if (datos_hechizo.lanzamientos_por_turno > 0 && total_hechizos_lanzados.ContainsKey(hechizo_id) && total_hechizos_lanzados[hechizo_id] >= datos_hechizo.lanzamientos_por_turno)
                return FallosLanzandoHechizo.DEMASIADOS_LANZAMIENTOS;

            if (hechizos_intervalo.ContainsKey(hechizo_id))
                return FallosLanzandoHechizo.COOLDOWN;

            if (datos_hechizo.efectos_normales.Count > 0 && datos_hechizo.efectos_normales[0].id == 181 && contador_invocaciones >= account.game.character.caracteristicas.criaturas_invocables.total_stats)
                return FallosLanzandoHechizo.DEMASIADAS_INVOCACIONES;

            return FallosLanzandoHechizo.NINGUNO;
        }

        public FallosLanzandoHechizo get_Puede_Lanzar_hechizo(short hechizo_id, Cell celda_actual, Cell celda_objetivo, Map mapa)
        {
            if(hechizo_id == 413)
            {
                return FallosLanzandoHechizo.NINGUNO;
            }

            Spell hechizo = account.game.character.get_Hechizo(hechizo_id);

            if (hechizo == null)
                return FallosLanzandoHechizo.DESONOCIDO;
            
            SpellStats datos_hechizo = hechizo.get_Stats();

            if (datos_hechizo.lanzamientos_por_objetivo > 0 && total_hechizos_lanzados_en_celda.ContainsKey(hechizo_id) && total_hechizos_lanzados_en_celda[hechizo_id].ContainsKey(celda_objetivo.cellId) && total_hechizos_lanzados_en_celda[hechizo_id][celda_objetivo.cellId] >= datos_hechizo.lanzamientos_por_objetivo)
                return FallosLanzandoHechizo.DEMASIADOS_LANZAMIENTOS_POR_OBJETIVO;

            if (datos_hechizo.es_celda_vacia && !es_Celda_Libre(celda_objetivo))
                return FallosLanzandoHechizo.NECESITA_CELDA_LIBRE;

            if (datos_hechizo.es_lanzado_linea && !jugador_luchador.celda.AreCellsOnLine(celda_objetivo))
                return FallosLanzandoHechizo.NO_ESTA_EN_LINEA;

            if (!get_Rango_hechizo(celda_actual, datos_hechizo, mapa).Contains(celda_objetivo.cellId))
                return FallosLanzandoHechizo.NO_ESTA_EN_RANGO;

            return FallosLanzandoHechizo.NINGUNO;
        }

        public List<short> get_Rango_hechizo(Cell celda_personaje, SpellStats datos_hechizo, Map mapa)
        {
            List<short> rango = new List<short>();
            
            foreach (Cell celda in SpellShape.Get_Lista_Celdas_Rango_Hechizo(celda_personaje, datos_hechizo, account.game.map, account.game.character.caracteristicas.alcanze.total_stats))
            {
                if (celda == null || rango.Contains(celda.cellId))
                    continue;

                if (datos_hechizo.es_celda_vacia && get_Celdas_Ocupadas.Contains(celda.cellId))
                    continue;

                if (celda.cellType != CellTypes.NOT_WALKABLE || celda.cellType != CellTypes.INTERACTIVE_OBJECT)
                    rango.Add(celda.cellId);
            }

            if (datos_hechizo.es_lanzado_con_vision)
            {
                for (int i = rango.Count - 1; i >= 0; i--)
                {
                    if (get_Linea_Obstruida(mapa, celda_personaje, mapa.GetCellFromId(rango[i]), get_Celdas_Ocupadas))
                        rango.RemoveAt(i);
                }
            }
            return rango;
        }

        public static bool get_Linea_Obstruida(Map mapa, Cell celda_inicial, Cell celda_destino, List<short> celdas_ocupadas)
        {
            double x = celda_inicial.x + 0.5;
            double y = celda_inicial.y + 0.5;
            double objetivo_x = celda_destino.x + 0.5;
            double objetivo_y = celda_destino.y + 0.5;
            double anterior_x = celda_inicial.x;
            double anterior_y = celda_inicial.y;
            int tipo = 0;

            double pasos;
            double pad_y;

            double pad_x;
            if (Math.Abs(x - objetivo_x) == Math.Abs(y - objetivo_y))
            {
                pasos = Math.Abs(x - objetivo_x);
                pad_x = (objetivo_x > x) ? 1 : -1;
                pad_y = (objetivo_y > y) ? 1 : -1;
                tipo = 1;
            }
            else if (Math.Abs(x - objetivo_x) > Math.Abs(y - objetivo_y))
            {
                pasos = Math.Abs(x - objetivo_x);
                pad_x = (objetivo_x > x) ? 1 : -1;
                pad_y = (objetivo_y - y) / pasos;
                pad_y = pad_y * 100;
                pad_y = Math.Ceiling(pad_y) / 100;
                tipo = 2;
            }
            else
            {
                pasos = Math.Abs(y - objetivo_y);
                pad_x = (objetivo_x - x) / pasos;
                pad_x = pad_x * 100;
                pad_x = Math.Ceiling(pad_x) / 100;
                pad_y = (objetivo_y > y) ? 1 : -1;
                tipo = 3;
            }

            int error_superior = Convert.ToInt32(Math.Round(Math.Floor(Convert.ToDouble((3 + (pasos / 2))))));
            int error_info = Convert.ToInt32(Math.Round(Math.Floor(Convert.ToDouble((97 - (pasos / 2))))));

            for (int i = 0; i < pasos; i++)
            {
                double cellX, cellY;
                double xPadX = x + pad_x;
                double yPadY = y + pad_y;

                switch (tipo)
                {
                    case 2:
                        double beforeY = Math.Ceiling(y * 100 + pad_y * 50) / 100;
                        double afterY = Math.Floor(y * 100 + pad_y * 150) / 100;
                        double diffBeforeCenterY = Math.Floor(Math.Abs(Math.Floor(beforeY) * 100 - beforeY * 100)) / 100;
                        double diffCenterAfterY = Math.Ceiling(Math.Abs(Math.Ceiling(afterY) * 100 - afterY * 100)) / 100;

                        cellX = Math.Floor(xPadX);

                        if (Math.Floor(beforeY) == Math.Floor(afterY))
                        {
                            cellY = Math.Floor(yPadY);
                            if ((beforeY == cellY && afterY < cellY) || (afterY == cellY && beforeY < cellY))
                            {
                                cellY = Math.Ceiling(yPadY);
                            }
                            if (get_Es_Celda_Obstruida(cellX, cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = cellX;
                            anterior_y = cellY;
                        }
                        else if (Math.Ceiling(beforeY) == Math.Ceiling(afterY))
                        {
                            cellY = Math.Ceiling(yPadY);
                            if ((beforeY == cellY && afterY < cellY) || (afterY == cellY && beforeY < cellY))
                            {
                                cellY = Math.Floor(yPadY);
                            }

                            if (get_Es_Celda_Obstruida(cellX, cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                                return true;

                            anterior_x = cellX;
                            anterior_y = cellY;
                        }
                        else if (Math.Floor(diffBeforeCenterY * 100) <= error_superior)
                        {
                            if (get_Es_Celda_Obstruida(cellX, Math.Floor(afterY), mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = cellX;
                            anterior_y = Math.Floor(afterY);
                        }
                        else if (Math.Floor(diffCenterAfterY * 100) >= error_info)
                        {
                            if (get_Es_Celda_Obstruida(cellX, Math.Floor(beforeY), mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = cellX;
                            anterior_y = Math.Floor(beforeY);
                        }
                        else
                        {
                            if (get_Es_Celda_Obstruida(cellX, Math.Floor(beforeY), mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = cellX;
                            anterior_y = Math.Floor(beforeY);
                            if (get_Es_Celda_Obstruida(cellX, Math.Floor(afterY), mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_y = Math.Floor(afterY);
                        }
                        break;

                    case 3:
                        double beforeX = Math.Ceiling(x * 100 + pad_x * 50) / 100;
                        double afterX = Math.Floor(x * 100 + pad_x * 150) / 100;
                        double diffBeforeCenterX = Math.Floor(Math.Abs(Math.Floor(beforeX) * 100 - beforeX * 100)) / 100;
                        double diffCenterAfterX = Math.Ceiling(Math.Abs(Math.Ceiling(afterX) * 100 - afterX * 100)) / 100;

                        cellY = Math.Floor(yPadY);

                        if (Math.Floor(beforeX) == Math.Floor(afterX))
                        {
                            cellX = Math.Floor(xPadX);
                            if ((beforeX == cellX && afterX < cellX) || (afterX == cellX && beforeX < cellX))
                            {
                                cellX = Math.Ceiling(xPadX);
                            }
                            if (get_Es_Celda_Obstruida(cellX, cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                                return true;
                            anterior_x = cellX;
                            anterior_y = cellY;
                        }
                        else if (Math.Ceiling(beforeX) == Math.Ceiling(afterX))
                        {
                            cellX = Math.Ceiling(xPadX);

                            if ((beforeX == cellX && afterX < cellX) || (afterX == cellX && beforeX < cellX))
                                cellX = Math.Floor(xPadX);

                            if (get_Es_Celda_Obstruida(cellX, cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                                return true;

                            anterior_x = cellX;
                            anterior_y = cellY;
                        }
                        else if (Math.Floor(diffBeforeCenterX * 100) <= error_superior)
                        {
                            if (get_Es_Celda_Obstruida(Math.Floor(afterX), cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                                return true;

                            anterior_x = Math.Floor(afterX);
                            anterior_y = cellY;
                        }
                        else if (Math.Floor(diffCenterAfterX * 100) >= error_info)
                        {
                            if (get_Es_Celda_Obstruida(Math.Floor(beforeX), cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                                return true;

                            anterior_x = Math.Floor(beforeX);
                            anterior_y = cellY;
                        }
                        else
                        {
                            if (get_Es_Celda_Obstruida(Math.Floor(beforeX), cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = Math.Floor(beforeX);
                            anterior_y = cellY;
                            if (get_Es_Celda_Obstruida(Math.Floor(afterX), cellY, mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y)) return true;
                            anterior_x = Math.Floor(afterX);
                        }
                        break;

                    default:
                        if (get_Es_Celda_Obstruida(Math.Floor(xPadX), Math.Floor(yPadY), mapa, celdas_ocupadas, celda_destino.cellId, anterior_x, anterior_y))
                            return true;
                        anterior_x = Math.Floor(xPadX);
                        anterior_y = Math.Floor(yPadY);
                        break;
                }

                x = (x * 100 + pad_x * 100) / 100;
                y = (y * 100 + pad_y * 100) / 100;
            }
            return false;
        }

        private static bool get_Es_Celda_Obstruida(double x, double y, Map map, List<short> occupiedCells, int targetCellId, double lastX, double lastY)
        {
            Cell mp = map.GetCellByCoordinates((int)x, (int)y);

            return mp.isInLineOfSight || (mp.cellId != targetCellId && occupiedCells.Contains(mp.cellId));
        }

        #region Zona Eventos
        public async Task FightCreatedAsync()
        {
            account.game.character.timer_regeneracion.Change(Timeout.Infinite, Timeout.Infinite);
            account.AccountState = AccountStates.FIGHTING;
            if (account.hasGroup && account.isGroupLeader)
            {

                var id = account.group.lider.game.character.id;
                foreach (var groupMember in account.group.members)
                {
                    if(groupMember.game.map.mapId == account.game.map.mapId)
                    {
                        groupMember.Logger.LogInfo("Fight", "Je rejoins le combat du leader");
                        var t = new Random().Next(800, 1450);
                        await Task.Delay(t);
                        groupMember.connexion.SendPacket("GA903" + id + ";" + id);
                    }
                    else
                    {
                        groupMember.Logger.LogError("Fight", "Je ne peux pas rejoindre le combat du leader qui se trouve dans une autre map !");
                    }
                }
            }
            pelea_creada?.Invoke();
            account.Logger.LogInfo("COMBAT", "Un nouveau combat a commencé");
        }

        public void Fightfinished(int xp , string time)
        {
            Clear();
            fightFinished?.Invoke();
            account.AccountState = AccountStates.CONNECTED_INACTIVE;
            if(account.isGroupLeader == true)
            account.Logger.LogInfo("COMBAT", "Combat terminé , Temps combat : " + time + " , XP gagné : " + xp);
            else
            {
                account.Logger.LogInfo("COMBAT", "Combat terminé , XP gagné : " + xp);
            }
        }

        public void Clear()
        {
            enemigos.Clear();
            aliados.Clear();
            luchadores.Clear();
            hechizos_intervalo.Clear();
            total_hechizos_lanzados.Clear();
            total_hechizos_lanzados_en_celda.Clear();
            celdas_preparacion.Clear();
            jugador_luchador = null;
        }

        public void get_Turno_Iniciado() => turno_iniciado?.Invoke();
        public void get_Hechizo_Lanzado(short celda_id, bool exito) => hechizo_lanzado?.Invoke(celda_id, exito);
        public void get_Movimiento_Exito(bool exito) => movimiento?.Invoke(exito);

        public void get_Turno_Acabado()
        {
            total_hechizos_lanzados.Clear();
            total_hechizos_lanzados_en_celda.Clear();

            for (int i = hechizos_intervalo.Count - 1; i >= 0; i--)
            {
                int key = hechizos_intervalo.ElementAt(i).Key;
                hechizos_intervalo[key]--;
                if (hechizos_intervalo[key] == 0)
                    hechizos_intervalo.Remove(key);
            }
        }
        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Fight() => Dispose(false);
        
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                luchadores.Clear();
                enemigos.Clear();
                aliados.Clear();
                total_hechizos_lanzados.Clear();
                hechizos_intervalo.Clear();
                total_hechizos_lanzados_en_celda.Clear();
                celdas_preparacion.Clear();
                account = null;
                luchadores = null;
                enemigos = null;
                aliados = null;
                total_hechizos_lanzados = null;
                hechizos_intervalo = null;
                total_hechizos_lanzados_en_celda = null;
                jugador_luchador = null;
                celdas_preparacion = null;
                disposed = true;
            }
        }
        #endregion
    }
}