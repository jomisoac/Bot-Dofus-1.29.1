using System;
using System.Collections.Generic;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas
{
    public class Pathfinder : IDisposable
    {
        private Cell[] celdas { get; set; }
        private Map mapa { get; set; }
        private bool disposed;

        public void set_Mapa(Map _mapa)
        {
            mapa = _mapa;
            celdas = mapa.mapCells;
        }

        public List<Cell> get_Path(Cell celda_inicio, Cell celda_final, List<Cell> celdas_no_permitidas, bool detener_delante, byte distancia_detener)
        {
            if (celda_inicio == null || celda_final == null)
                return null;

            List<Cell> celdas_permitidas = new List<Cell>() { celda_inicio };

            if (celdas_no_permitidas.Contains(celda_final))
                celdas_no_permitidas.Remove(celda_final);

            while (celdas_permitidas.Count > 0)
            {
                int index = 0;
                for (int i = 1; i < celdas_permitidas.Count; i++)
                {
                    if (celdas_permitidas[i].coste_f < celdas_permitidas[index].coste_f)
                        index = i;

                    if (celdas_permitidas[i].coste_f != celdas_permitidas[index].coste_f) continue;
                    if (celdas_permitidas[i].coste_g > celdas_permitidas[index].coste_g)
                        index = i;
                    
                    if (celdas_permitidas[i].coste_g == celdas_permitidas[index].coste_g)
                        index = i;
                    
                    if (celdas_permitidas[i].coste_g == celdas_permitidas[index].coste_g)
                        index = i;
                }

                Cell actual = celdas_permitidas[index];

                if (detener_delante && get_Distancia_Nodos(actual, celda_final) <= distancia_detener && !celda_final.IsWalkable())
                    return get_Camino_Retroceso(celda_inicio, actual);

                if (actual == celda_final)
                    return get_Camino_Retroceso(celda_inicio, celda_final);
                
                celdas_permitidas.Remove(actual);
                celdas_no_permitidas.Add(actual);

                foreach (Cell celda_siguiente in get_Celdas_Adyecentes(actual))
                {
                    if (celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.IsWalkable())
                        continue;

                    if (celda_siguiente.IsTeleportCell() && celda_siguiente != celda_final)
                       continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);

                    if (!celdas_permitidas.Contains(celda_siguiente))
                        celdas_permitidas.Add(celda_siguiente);
                    else if (temporal_g >= celda_siguiente.coste_g)
                        continue;

                    celda_siguiente.coste_g = temporal_g;
                    celda_siguiente.coste_h = get_Distancia_Nodos(celda_siguiente, celda_final);
                    celda_siguiente.coste_f = celda_siguiente.coste_g + celda_siguiente.coste_h;
                    celda_siguiente.parentNode = actual;
                }
            }

            return null;
        }

        private List<Cell> get_Camino_Retroceso(Cell nodo_inicial, Cell nodo_final)
        {
            Cell nodo_actual = nodo_final;
            List<Cell> celdas_camino = new List<Cell>();

            while (nodo_actual != nodo_inicial)
            {
                celdas_camino.Add(nodo_actual);
                nodo_actual = nodo_actual.parentNode;
            }

            celdas_camino.Add(nodo_inicial);
            celdas_camino.Reverse();

            return celdas_camino;
        }
        
        public List<Cell> get_Celdas_Adyecentes(Cell nodo)
        {
            List<Cell> celdas_adyecentes = new List<Cell>();

            Cell celda_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y);
            Cell celda_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y);
            Cell celda_inferior = celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y + 1);
            Cell celda_superior = celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y - 1);

            if (celda_derecha != null)
                celdas_adyecentes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_adyecentes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_adyecentes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_adyecentes.Add(celda_superior);

            Cell superior_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y - 1);
            Cell inferior_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y + 1);
            Cell inferior_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y + 1);
            Cell superior_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y - 1);

            if (superior_izquierda != null)
                celdas_adyecentes.Add(superior_izquierda);
            if (superior_derecha != null)
                celdas_adyecentes.Add(superior_derecha);
            if (inferior_derecha != null)
                celdas_adyecentes.Add(inferior_derecha);
            if (inferior_izquierda != null)
                celdas_adyecentes.Add(inferior_izquierda);

            return celdas_adyecentes;
        }

        private int get_Distancia_Nodos(Cell a, Cell b) => ((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y));

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Pathfinder() => Dispose(false);
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                celdas = null;
                mapa = null;
                disposed = true;
            }
        }
        #endregion
    }
}
