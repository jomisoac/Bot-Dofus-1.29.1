using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class Pelea
    {
        private Cuenta cuenta;
        private ConcurrentDictionary<int, Luchadores> luchadores;
        private ConcurrentDictionary<int, Luchadores> enemigos;
        private ConcurrentDictionary<int, Luchadores> aliados;
        private Dictionary<int, int> total_hechizos_lanzados;//id-veces

        public LuchadorPersonaje jugador_luchador { get; private set; }

        public IEnumerable<Luchadores> get_Aliados => aliados.Values.Where(a => a.esta_vivo);
        public IEnumerable<Luchadores> get_Enemigos => enemigos.Values.Where(e => e.esta_vivo);
        public IEnumerable<Luchadores> get_Luchadores => luchadores.Values.Where(f => f.esta_vivo);
        public int total_enemigos_vivos => get_Enemigos.Count(f => f.esta_vivo);
        public List<int> get_Celdas_Ocupadas => get_Luchadores.Select(f => f.celda_id).ToList();

        public Pelea(Cuenta _cuenta)
        {
            cuenta = _cuenta;
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

        public void get_Agregar_Luchador(Luchadores luchador)
        {
            if (luchador.id == cuenta.personaje.id)
            {
                jugador_luchador = new LuchadorPersonaje(cuenta.personaje.nombre_personaje, cuenta.personaje.nivel, luchador);
            }
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
    }
}