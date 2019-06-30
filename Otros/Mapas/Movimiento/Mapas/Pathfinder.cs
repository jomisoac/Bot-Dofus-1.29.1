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
        private Celda[] celdas { get; set; }
        private Mapa mapa { get; set; }
        private bool disposed;

        public void set_Mapa(Mapa _mapa)
        {
            mapa = _mapa;
            celdas = mapa.celdas;
        }

        public List<Celda> get_Path(Celda celda_inicio, Celda celda_final, List<Celda> celdas_no_permitidas, bool detener_delante, byte distancia_detener)
        {
            if (celda_inicio == null || celda_final == null)
                return null;

            List<Celda> celdas_permitidas = new List<Celda>() { celda_inicio };

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

                Celda actual = celdas_permitidas[index];

                if (detener_delante && get_Distancia_Nodos(actual, celda_final) <= distancia_detener && !celda_final.es_Caminable())
                    return get_Camino_Retroceso(celda_inicio, actual);

                if (actual == celda_final)
                    return get_Camino_Retroceso(celda_inicio, celda_final);
                
                celdas_permitidas.Remove(actual);
                celdas_no_permitidas.Add(actual);

                foreach (Celda celda_siguiente in get_Celdas_Adyecentes(actual))
                {
                    if (celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.es_Caminable())
                        continue;

                    if (celda_siguiente.es_Teleport() && celda_siguiente != celda_final)
                       continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);

                    if (!celdas_permitidas.Contains(celda_siguiente))
                        celdas_permitidas.Add(celda_siguiente);
                    else if (temporal_g >= celda_siguiente.coste_g)
                        continue;

                    celda_siguiente.coste_g = temporal_g;
                    celda_siguiente.coste_h = get_Distancia_Nodos(celda_siguiente, celda_final);
                    celda_siguiente.coste_f = celda_siguiente.coste_g + celda_siguiente.coste_h;
                    celda_siguiente.nodo_padre = actual;
                }
            }

            return null;
        }

        private List<Celda> get_Camino_Retroceso(Celda nodo_inicial, Celda nodo_final)
        {
            Celda nodo_actual = nodo_final;
            List<Celda> celdas_camino = new List<Celda>();

            while (nodo_actual != nodo_inicial)
            {
                celdas_camino.Add(nodo_actual);
                nodo_actual = nodo_actual.nodo_padre;
            }

            celdas_camino.Add(nodo_inicial);
            celdas_camino.Reverse();

            return celdas_camino;
        }
        
        public List<Celda> get_Celdas_Adyecentes(Celda nodo)
        {
            List<Celda> celdas_adyecentes = new List<Celda>();

            Celda celda_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y);
            Celda celda_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y);
            Celda celda_inferior = celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y + 1);
            Celda celda_superior = celdas.FirstOrDefault(nodec => nodec.x == nodo.x && nodec.y == nodo.y - 1);

            if (celda_derecha != null)
                celdas_adyecentes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_adyecentes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_adyecentes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_adyecentes.Add(celda_superior);

            Celda superior_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y - 1);
            Celda inferior_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y + 1);
            Celda inferior_izquierda = celdas.FirstOrDefault(nodec => nodec.x == nodo.x - 1 && nodec.y == nodo.y + 1);
            Celda superior_derecha = celdas.FirstOrDefault(nodec => nodec.x == nodo.x + 1 && nodec.y == nodo.y - 1);

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

        private int get_Distancia_Nodos(Celda a, Celda b) => ((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y));

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
