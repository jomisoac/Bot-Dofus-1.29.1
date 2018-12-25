using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class Pelea : IDisposable
    {
        private Cuenta cuenta;
        private Mapa mapa;
        private ConcurrentDictionary<int, Luchadores> luchadores;
        private ConcurrentDictionary<int, Luchadores> enemigos;
        private ConcurrentDictionary<int, Luchadores> aliados;
        private Dictionary<int, int> hechizos_intervalo;// hechizoID, turnos intervalo
        private Dictionary<int, int> total_hechizos_lanzados;//hechizoID, total veces
        private Dictionary<int, Dictionary<int, int>> total_hechizos_lanzados_en_casilla;//hechizoID (celda, veces)
        private bool disposed;

        public LuchadorPersonaje jugador_luchador { get; private set; }

        public IEnumerable<Luchadores> get_Aliados => aliados.Values.Where(a => a.esta_vivo);
        public IEnumerable<Luchadores> get_Enemigos => enemigos.Values.Where(e => e.esta_vivo);
        public IEnumerable<Luchadores> get_Luchadores => luchadores.Values.Where(f => f.esta_vivo);
        public int total_enemigos_vivos => get_Enemigos.Count(f => f.esta_vivo);
        public List<int> get_Celdas_Ocupadas => get_Luchadores.Select(f => f.celda_id).ToList();

        public Pelea(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            mapa = cuenta.personaje.mapa;
            luchadores = new ConcurrentDictionary<int, Luchadores>();
            enemigos = new ConcurrentDictionary<int, Luchadores>();
            aliados = new ConcurrentDictionary<int, Luchadores>();
            hechizos_intervalo = new Dictionary<int, int>();
            total_hechizos_lanzados = new Dictionary<int, int>();
            total_hechizos_lanzados_en_casilla = new Dictionary<int, Dictionary<int, int>>();
        }

        public void get_Lanzar_Hechizo(int hechizo_id, int celda_id)
        {
            if (cuenta.Estado_Cuenta == Protocolo.Enums.EstadoCuenta.LUCHANDO)
            {
                Hechizo hechizo = cuenta.personaje.hechizos.FirstOrDefault(f => f.id == hechizo_id);
                HechizoStats datos_hechizo = hechizo.get_Hechizo_Stats()[hechizo.nivel];

                cuenta.conexion.enviar_Paquete("GA300" + hechizo.id + ';' + celda_id);

                if (datos_hechizo.intervalo > 0 && !hechizos_intervalo.ContainsKey(hechizo.id))
                {
                    hechizos_intervalo.Add(hechizo.id, datos_hechizo.intervalo);
                }

                if (!total_hechizos_lanzados.ContainsKey(hechizo.id))
                    total_hechizos_lanzados.Add(hechizo.id, 0);
                total_hechizos_lanzados[hechizo.id]++;

                if (total_hechizos_lanzados_en_casilla.ContainsKey(hechizo.id))
                {
                    if (!total_hechizos_lanzados_en_casilla[hechizo.id].ContainsKey(celda_id))
                        total_hechizos_lanzados_en_casilla[hechizo.id].Add(celda_id, 0);
                    total_hechizos_lanzados_en_casilla[hechizo.id][celda_id]++;
                }
                else
                {
                    total_hechizos_lanzados_en_casilla.Add(hechizo.id, new Dictionary<int, int>()
                    {
                        { celda_id, 1 }
                    });
                }
            }  
        }

        public void get_Final_Turno(int id_personaje)
        {
            Luchadores luchador = get_Luchador_Por_Id(id_personaje);

            if (luchador == jugador_luchador)
            {
                total_hechizos_lanzados.Clear();
                total_hechizos_lanzados_en_casilla.Clear();
                
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
            {
                return jugador_luchador;
            }
            else if (luchadores.TryGetValue(id, out Luchadores luchador))
            {
                return luchador;
            }
            else
            {
                return null;
            }
        }

        public Luchadores get_Enemigo_Mas_Debil()
        {
            int vida = -1;
            Luchadores enemigo = null;

            foreach (Luchadores luchador_enemigo in get_Enemigos)
            {
                if (!luchador_enemigo.esta_vivo)
                    continue;

                if (vida == -1 || luchador_enemigo.porcentaje_vida < vida)
                {
                    vida = luchador_enemigo.porcentaje_vida;
                    enemigo = luchador_enemigo;
                }
            }
            return enemigo;
        }

        public Luchadores get_Obtener_Aliado_Mas_Cercano()
        {
            int distancia = -1, distancia_temporal;
            Luchadores aliado = null;

            foreach (var luchador_aliado in get_Aliados)
            {
                if (!luchador_aliado.esta_vivo)
                    continue;

                distancia_temporal = cuenta.personaje.mapa.get_Distancia_Entre_Dos_Casillas(jugador_luchador.celda_id, luchador_aliado.celda_id);

                if (distancia == -1 || distancia_temporal < distancia)
                {
                    distancia = distancia_temporal;
                    aliado = luchador_aliado;
                }
            }
            return aliado;
        }

        public Luchadores get_Obtener_enemigo_Mas_Cercano()
        {
            int distancia = -1, distancia_temporal;
            Luchadores enemigo = null;

            foreach (var luchador_enemigo in get_Enemigos)
            {
                if (!luchador_enemigo.esta_vivo)
                    continue;

                distancia_temporal = cuenta.personaje.mapa.get_Distancia_Entre_Dos_Casillas(jugador_luchador.celda_id, luchador_enemigo.celda_id);

                if (distancia == -1 || distancia_temporal < distancia)
                {
                    distancia = distancia_temporal;
                    enemigo = luchador_enemigo;
                }
            }
            return enemigo;
        }

        public void get_Agregar_Luchador(Luchadores luchador)
        {
            if (luchador.id == cuenta.personaje.id)
                jugador_luchador = new LuchadorPersonaje(cuenta.personaje.nombre_personaje, cuenta.personaje.nivel, luchador);
            else
            {
                if (!luchadores.TryAdd(luchador.id, luchador))
                {
                    luchadores[luchador.id].get_Actualizar_Luchador(luchador);
                }
            }
            get_Ordenar_Luchadores();
        }

        private void get_Ordenar_Luchadores()
        {
            foreach (Luchadores luchador in get_Luchadores)
            {
                if (aliados.ContainsKey(luchador.id) || enemigos.ContainsKey(luchador.id))
                    continue;

                if (luchador.equipo == jugador_luchador.equipo)
                {
                    aliados.TryAdd(luchador.id, luchador);
                }
                else
                {
                    enemigos.TryAdd(luchador.id, luchador);
                }
            }
        }

        public Luchadores get_Luchador_Esta_En_Celda(int celda_id)
        {
            if (jugador_luchador?.celda_id == celda_id)
                return jugador_luchador;
            return get_Luchadores.FirstOrDefault(f => f.esta_vivo && f.celda_id == celda_id);
        }

        public bool es_Celda_Libre(int celda_id) => get_Luchador_Esta_En_Celda(celda_id) == null;

        public IEnumerable<Luchadores> get_Cuerpo_A_Cuerpo_Enemigo(int celda_id = -1) => get_Enemigos.Where(e => e.esta_vivo && mapa.get_Distancia_Entre_Dos_Casillas(celda_id == -1 ? jugador_luchador.celda_id : celda_id, e.celda_id) == 1);
        public IEnumerable<Luchadores> get_Cuerpo_A_Cuerpo_Aliado(int celda_id = -1) => get_Aliados.Where(a => a.esta_vivo && mapa.get_Distancia_Entre_Dos_Casillas(celda_id == -1 ? jugador_luchador.celda_id : celda_id, a.celda_id) == 1);
        public bool esta_Cuerpo_A_Cuerpo_Con_Enemigo(int celda_id = -1) => get_Cuerpo_A_Cuerpo_Enemigo(celda_id).Count() > 0;
        public bool esta_Cuerpo_A_Cuerpo_Con_Aliado(int celda_id = -1) => get_Cuerpo_A_Cuerpo_Aliado(celda_id).Count() > 0;

        public FallosLanzandoHechizo get_Puede_Lanzar_hechizo(int hechizo_id)
        {
            Hechizo hechizo = cuenta.personaje.hechizos.FirstOrDefault(f => f.id == hechizo_id);

            if (hechizo == null)
                return FallosLanzandoHechizo.DESONOCIDO;

            HechizoStats datos_hechizo = hechizo.get_Hechizo_Stats()[hechizo.nivel];

            if (jugador_luchador.pa < datos_hechizo.coste_pa)
                return FallosLanzandoHechizo.PUNTOS_ACCION;

            if (datos_hechizo.lanzamientos_por_turno > 0 && total_hechizos_lanzados.ContainsKey(hechizo_id) && total_hechizos_lanzados[hechizo_id] >= datos_hechizo.lanzamientos_por_turno)
                return FallosLanzandoHechizo.DEMASIADOS_LANZAMIENTOS;

            if (hechizos_intervalo.ContainsKey(hechizo_id))
                return FallosLanzandoHechizo.COOLDOWN;

            return FallosLanzandoHechizo.NINGUNO;
        }

        public FallosLanzandoHechizo get_Puede_Lanzar_hechizo(int hechizo_id, int personaje_celda, int celda_objetivo)
        {
            Hechizo hechizo = cuenta.personaje.hechizos.FirstOrDefault(f => f.id == hechizo_id);

            if (hechizo == null)
                return FallosLanzandoHechizo.DESONOCIDO;

            HechizoStats datos_hechizo = hechizo.get_Hechizo_Stats()[hechizo.nivel];

            if (datos_hechizo.lanzamientos_por_objetivo > 0 && total_hechizos_lanzados_en_casilla.ContainsKey(hechizo_id) && total_hechizos_lanzados_en_casilla[hechizo_id].ContainsKey(celda_objetivo) && total_hechizos_lanzados_en_casilla[hechizo_id][celda_objetivo] >= datos_hechizo.lanzamientos_por_objetivo)
                return FallosLanzandoHechizo.DEMASIADOS_LANZAMIENTOS_POR_OBJETIVO;

            if (datos_hechizo.es_celda_vacia && !es_Celda_Libre(celda_objetivo))
                return FallosLanzandoHechizo.NECESITA_CELDA_LIBRE;

            return FallosLanzandoHechizo.NINGUNO;
        }

        ~Pelea() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                luchadores.Clear();
                enemigos.Clear();
                aliados.Clear();
                total_hechizos_lanzados.Clear();
                hechizos_intervalo.Clear();
                total_hechizos_lanzados_en_casilla.Clear();
                cuenta = null;
                luchadores = null;
                enemigos = null;
                aliados = null;
                total_hechizos_lanzados = null;
                hechizos_intervalo = null;
                total_hechizos_lanzados_en_casilla = null;
                disposed = true;
            }
        }
    }
}