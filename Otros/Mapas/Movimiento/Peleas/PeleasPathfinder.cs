using Bot_Dofus_1._29._1.Otros.Peleas;
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
        private static void rellenar_cuadricula(Mapa mapa, out NodoCeldas[] cuadricula)
        {
            cuadricula = new NodoCeldas[mapa.celdas.Length];

            foreach (Celda celda in mapa.celdas)
                cuadricula[celda.id] = new NodoCeldas(celda.id, mapa.get_Celda_X_Coordenadas(celda.id), mapa.get_Celda_Y_Coordenadas(celda.id), celda.tipo != TipoCelda.NO_CAMINABLE && celda.tipo != TipoCelda.OBJETO_INTERACTIVO || celda.objeto_interactivo_id != -1);
        }

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
                var cell = celdas[actual];
                if (cell.alcanzable)
                {
                    celdas_accesibles.Insert(0, actual);
                    lista_mapa_alcanzables.Add(actual, distancia);
                }
                else
                {
                    celdas_inalcanzables.Insert(0, actual);
                    lista_mapa_inalcanzable.Add(actual, distancia);
                }

                actual = cell.celda_inicial;
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

        public static Dictionary<short, MovimientoNodo> get_Celdas_Accesibles(Pelea pelea, Mapa mapa, short celda_actual)
        {
            Dictionary<short, MovimientoNodo> celdas = new Dictionary<short, MovimientoNodo>();

            if (pelea.jugador_luchador.pm <= 0)
                return celdas;

            NodoCeldas[] cuadricula;
            rellenar_cuadricula(mapa, out cuadricula);

            short maximos_pm = pelea.jugador_luchador.pm;

            List<NodoPelea> celdas_permitidas = new List<NodoPelea>();
            Dictionary<short, NodoPelea> celdas_prohibidas = new Dictionary<short, NodoPelea>();
            
            NodoPelea nodo = new NodoPelea(celda_actual, maximos_pm, pelea.jugador_luchador.pa, 1);
            celdas_permitidas.Add(nodo);
            celdas_prohibidas[celda_actual] = nodo;
            
            while (celdas_permitidas.Count > 0)
            {
                NodoPelea actual = celdas_permitidas.Last();
                celdas_permitidas.Remove(actual);
                short celda_id = actual.celda_id;
                List<NodoCeldas> adyecentes = get_Celdas_Siguiente(cuadricula[celda_id], cuadricula);

                int pm_disponibles = actual.pm_disponible - 1;
                int pa_disponibles = actual.pa_disponible;
                int distancia = actual.distancia + 1;
                bool accesible = pm_disponibles >= 0;

                for (int i = 0; i < adyecentes.Count; i++)
                {
                    if (celdas_prohibidas.ContainsKey(adyecentes[i].id))
                    {
                        NodoPelea anterior = celdas_prohibidas[adyecentes[i].id];
                        if (anterior.pm_disponible > pm_disponibles)
                            continue;

                        if (anterior.pm_disponible == pm_disponibles && anterior.pm_disponible >= pa_disponibles)
                            continue;
                    }

                    if (!adyecentes[i].es_caminable)
                        continue;

                    celdas[adyecentes[i].id] = new MovimientoNodo(celda_id, accesible);
                    nodo = new NodoPelea(adyecentes[i].id, pm_disponibles, pa_disponibles, distancia);
                    celdas_prohibidas[adyecentes[i].id] = nodo;

                    if (actual.distancia < maximos_pm)
                        celdas_permitidas.Add(nodo);
                }
            }

            foreach (short celda in celdas.Keys)
                celdas[celda].camino = get_Path_Pelea(celda_actual, celda, celdas);

            return celdas;
        }

        public static List<NodoCeldas> get_Celdas_Siguiente(NodoCeldas nodo, NodoCeldas[] cuadricula)/** es pelea no usa diagonales **/
        {
            List<NodoCeldas> celdas_siguientes = new List<NodoCeldas>();

            NodoCeldas celda_derecha = cuadricula.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x + 1 && nodec.posicion_y == nodo.posicion_y);
            NodoCeldas celda_izquierda = cuadricula.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x - 1 && nodec.posicion_y == nodo.posicion_y);
            NodoCeldas celda_inferior = cuadricula.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x && nodec.posicion_y == nodo.posicion_y + 1);
            NodoCeldas celda_superior = cuadricula.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x && nodec.posicion_y == nodo.posicion_y - 1);

            if (celda_derecha != null)
                celdas_siguientes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_siguientes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_siguientes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_siguientes.Add(celda_superior);

            return celdas_siguientes; 
        }
    }
}
