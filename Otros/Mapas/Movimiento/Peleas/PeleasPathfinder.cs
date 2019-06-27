using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using System.Collections.Generic;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas
{
    class PeleasPathfinder
    {
        public static PeleaCamino get_Path_Pelea(short celda_actual, short celda_objetivo, Dictionary<short, MovimientoNodo> celdas)
        {
            if (!celdas.ContainsKey(celda_objetivo))
                return null;

            short actual = celda_objetivo;
            List<short> celdas_accesibles = new List<short>();
            List<short> celdas_inalcanzables = new List<short>();
            Dictionary<short, int> lista_mapa_alcanzables = new Dictionary<short, int>();
            Dictionary<short, int> lista_mapa_inalcanzable = new Dictionary<short, int>();
            byte distancia = 0;

            while (actual != celda_actual)
            {
                MovimientoNodo celda = celdas[actual];
                if (celda.alcanzable)
                {
                    celdas_accesibles.Insert(0, actual);
                    lista_mapa_alcanzables.Add(actual, distancia);
                }
                else
                {
                    celdas_inalcanzables.Insert(0, actual);
                    lista_mapa_inalcanzable.Add(actual, distancia);
                }

                actual = celda.celda_inicial;
                distancia += 1;
            }

            return new PeleaCamino()
            {
                celdas_accesibles = celdas_accesibles,
                celdas_inalcanzables = celdas_inalcanzables,
                mapa_celdas_alcanzables = lista_mapa_alcanzables,
                mapa_celdas_inalcanzable = lista_mapa_inalcanzable,
            };
        }

        public static Dictionary<short, MovimientoNodo> get_Celdas_Accesibles(Pelea pelea, Mapa mapa, Celda celda_actual)
        {
            Dictionary<short, MovimientoNodo> celdas = new Dictionary<short, MovimientoNodo>();

            if (pelea.jugador_luchador.pm <= 0)
                return celdas;

            short maximos_pm = pelea.jugador_luchador.pm;

            List<NodoPelea> celdas_permitidas = new List<NodoPelea>();
            Dictionary<short, NodoPelea> celdas_prohibidas = new Dictionary<short, NodoPelea>();
            
            NodoPelea nodo = new NodoPelea(celda_actual, maximos_pm, pelea.jugador_luchador.pa, 1);
            celdas_permitidas.Add(nodo);
            celdas_prohibidas[celda_actual.id] = nodo;
            
            while (celdas_permitidas.Count > 0)
            {
                NodoPelea actual = celdas_permitidas.Last();
                celdas_permitidas.Remove(actual);
                Celda nodo_celda = actual.celda;
                List<Celda> adyecentes = get_Celdas_Adyecentes(nodo_celda, mapa.celdas);
                
                int i = 0;
                while (i < adyecentes.Count)
                {
                    Luchadores enemigo = pelea.get_Luchadores.FirstOrDefault(f => f.celda.id == adyecentes[i]?.id);

                    if (adyecentes[i] != null && enemigo == null)
                    {
                        i++;
                        continue;
                    }
                    adyecentes.RemoveAt(i);
                }

                int pm_disponibles = actual.pm_disponible - 1;
                int pa_disponibles = actual.pa_disponible;
                int distancia = actual.distancia + 1;
                bool accesible = pm_disponibles >= 0;

                for (i = 0; i < adyecentes.Count; i++)
                {
                    if (celdas_prohibidas.ContainsKey(adyecentes[i].id))
                    {
                        NodoPelea anterior = celdas_prohibidas[adyecentes[i].id];
                        if (anterior.pm_disponible > pm_disponibles)
                            continue;

                        if (anterior.pm_disponible == pm_disponibles && anterior.pm_disponible >= pa_disponibles)
                            continue;
                    }

                    if (!adyecentes[i].es_Caminable())
                        continue;

                    celdas[adyecentes[i].id] = new MovimientoNodo(nodo_celda.id, accesible);
                    nodo = new NodoPelea(adyecentes[i], pm_disponibles, pa_disponibles, distancia);
                    celdas_prohibidas[adyecentes[i].id] = nodo;

                    if (actual.distancia < maximos_pm)
                        celdas_permitidas.Add(nodo);
                }
            }

            foreach (short celda in celdas.Keys)
                celdas[celda].camino = get_Path_Pelea(celda_actual.id, celda, celdas);

            return celdas;
        }

        //pelea no utiliza diagonales
        public static List<Celda> get_Celdas_Adyecentes(Celda nodo, Celda[] mapa_celdas)
        {
            List<Celda> celdas_adyecentes = new List<Celda>();

            Celda celda_derecha = mapa_celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y);
            Celda celda_izquierda = mapa_celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y);
            Celda celda_inferior = mapa_celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y + 1);
            Celda celda_superior = mapa_celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y - 1);

            if (celda_derecha != null)
                celdas_adyecentes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_adyecentes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_adyecentes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_adyecentes.Add(celda_superior);

            return celdas_adyecentes;
        }
    }
}
