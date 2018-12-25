using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Mapas;
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
        private Dictionary<int, int> total_hechizos_lanzados;//id-veces
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
            total_hechizos_lanzados = new Dictionary<int, int>();
        }

        public void get_Lanzar_Hechizo(int hechizo_id, int celda_id)
        {
            if (cuenta.Estado_Cuenta == Protocolo.Enums.EstadoCuenta.LUCHANDO)
                cuenta.conexion.enviar_Paquete("GA300" + hechizo_id + ';' + celda_id);
            else
                return;
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

            foreach (var ennemyEntry in get_Enemigos)
            {
                if (!ennemyEntry.esta_vivo)
                    continue;

                if (vida == -1 || ennemyEntry.porcentaje_vida < vida)
                {
                    vida = ennemyEntry.porcentaje_vida;
                    enemigo = ennemyEntry;
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

        public Luchadores GetFighterInCell(int celda_id)
        {
            if (jugador_luchador?.celda_id == celda_id)
                return jugador_luchador;
            return get_Luchadores.FirstOrDefault(f => f.esta_vivo && f.celda_id == celda_id);
        }

        public bool es_Celda_Libre(int celda_id) => GetFighterInCell(celda_id) == null;

        public IEnumerable<Luchadores> get_Cuerpo_A_Cuerpo_Enemigo(int celda_id = -1)
        {
            return get_Enemigos.Where(e => e.esta_vivo && mapa.get_Distancia_Entre_Dos_Casillas(jugador_luchador.celda_id, e.celda_id) == 1);
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
                cuenta = null;
                luchadores = null;
                enemigos = null;
                aliados = null;
                total_hechizos_lanzados = null;
                disposed = true;
            }
        }
    }
}