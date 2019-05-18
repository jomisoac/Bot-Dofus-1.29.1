using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento
{
    public class Pathfinding : IDisposable
    {
        private Celda[] celdas { get; set; }
        public List<short> celdas_camino { get; private set; }
        private Mapa mapa { get; set; }

        public static int tiempo_desplazamiento = 0;
        private bool disposed;

        private static readonly Dictionary<TipoAnimacion, DuracionAnimacion> tiempo_tipo_animacion = new Dictionary<TipoAnimacion, DuracionAnimacion>()
        {
            { TipoAnimacion.MONTURA, new DuracionAnimacion(135, 200, 120) },
            { TipoAnimacion.CORRIENDO, new DuracionAnimacion(170, 255, 150) },
            { TipoAnimacion.CAMINANDO, new DuracionAnimacion(480, 510, 425) },
            { TipoAnimacion.FANTASMA, new DuracionAnimacion(57, 85, 50) }//speed hack ;)
        };

        public Pathfinding(Mapa _mapa)
        {
            mapa = _mapa;
            celdas = mapa.celdas;
            celdas_camino = new List<short>();
        }

        private void cargar_Obstaculos(ref List<Celda> celdas_no_permitidas)
        {
            foreach (Monstruo monstruo in mapa.get_Monstruos().Values)
                celdas_no_permitidas.Add(monstruo.celda);
        }

        public bool get_Puede_Caminar(short celda_inicio, short celda_final, bool esquivar_monstruos)
        {
            Celda inicio = celdas[celda_inicio];
            Celda final = celdas[celda_final];

            List<Celda> celdas_permitidas = new List<Celda>() { inicio };
            List<Celda> celdas_no_permitidas = new List<Celda>();

            if (esquivar_monstruos)
                cargar_Obstaculos(ref celdas_no_permitidas);

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
                if (actual == final)
                {
                    get_Camino_Retroceso(inicio, final);
                    return true;
                }
                celdas_permitidas.Remove(actual);
                celdas_no_permitidas.Add(actual);

                foreach (Celda celda_siguiente in get_Celdas_Adyecentes(actual))
                {
                    if (celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.es_Caminable() || celda_siguiente.tipo == TipoCelda.CELDA_TELEPORT && celda_siguiente != final)
                        continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);

                    if (!celdas_permitidas.Contains(celda_siguiente))
                        celdas_permitidas.Add(celda_siguiente);
                    else if (temporal_g >= celda_siguiente.coste_g)
                        continue;

                    celda_siguiente.coste_g = temporal_g;
                    celda_siguiente.coste_h = get_Distancia_Nodos(celda_siguiente, final);
                    celda_siguiente.coste_f = celda_siguiente.coste_g + celda_siguiente.coste_h;
                    celda_siguiente.nodo_padre = actual;
                }
            }
            return false;
        }

        private void get_Camino_Retroceso(Celda nodo_inicial, Celda nodo_final)
        {
            Celda nodo_actual = nodo_final;

            while (nodo_actual != nodo_inicial)
            {
                celdas_camino.Add(nodo_actual.id);
                nodo_actual = nodo_actual.nodo_padre;
            }

            celdas_camino.Add(nodo_inicial.id);
            celdas_camino.Reverse();
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

        public int get_Tiempo_Desplazamiento_Mapa(short casilla_actual, short casilla_final)
        {
            tiempo_desplazamiento = 20;
            DuracionAnimacion tipo_animacion = celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            short siguiente_celda;

            for (int i = 0; i < celdas_camino.Count - 1; i++)
            {
                siguiente_celda = celdas[celdas_camino[i + 1]].id;

                if (celdas[casilla_actual].y == celdas[siguiente_celda].y)
                    tiempo_desplazamiento += tipo_animacion.horizontal;
                else if (celdas[casilla_actual].x == celdas[siguiente_celda].y)
                    tiempo_desplazamiento += tipo_animacion.vertical;
                else
                    tiempo_desplazamiento += tipo_animacion.lineal;

                if (celdas[casilla_actual].layer_ground_nivel < celdas[siguiente_celda].layer_ground_nivel)
                    tiempo_desplazamiento += 100;
                else if (celdas[siguiente_celda].layer_ground_nivel > celdas[casilla_actual].layer_ground_nivel)
                    tiempo_desplazamiento -= 100;
                else if (celdas[casilla_actual].layer_ground_slope != celdas[siguiente_celda].layer_ground_slope)
                {
                    if (celdas[casilla_actual].layer_ground_slope == 1)
                        tiempo_desplazamiento += 100;
                    else if (celdas[siguiente_celda].layer_ground_slope == 1)
                        tiempo_desplazamiento -= 100;
                }
                casilla_actual = siguiente_celda;
            }

            return tiempo_desplazamiento;
        }

        private static char get_Direccion_Dos_Celdas(short celda_1, short celda_2, bool es_pelea, Mapa mapa)
        {
            if (celda_1 == celda_2 || mapa == null)
                return (char)0;

            Celda celda1 = mapa.celdas[celda_1], celda2 = mapa.celdas[celda_2];

            if(!es_pelea)
            {
                byte mapa_anchura = mapa.anchura;
                int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
                int _loc7_ = celda_2 - celda_1;

                for (int i = 7; i >= 0; i += -1)
                {
                    if (_loc6_[i] == _loc7_)
                        return (char)(i + 'a');
                }
            }

            int resultado_x = celda2.x - celda1.x;
            int resultado_y = celda2.y - celda1.y;

            if (resultado_x == 0)
            {
                if (resultado_y > 0)
                    return (char)(3 + 'a');
                else
                    return (char)(7 + 'a');
            }
            else if (resultado_x > 0)
                return (char)(1 + 'a');
            else
                return (char)(5 + 'a');
        }

        private int get_Distancia_Nodos(Celda a, Celda b) => ((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y));

        public static string get_Pathfinding_Limpio(List<short> celdas_camino, bool es_pelea, Mapa mapa)
        {
            StringBuilder pathfinding_limpio = new StringBuilder(), camino = new StringBuilder();

            for (int i = 0; i < celdas_camino.Count - 1; i++)
                camino.Append(get_Direccion_Dos_Celdas(celdas_camino[i], celdas_camino[i + 1], es_pelea, mapa)).Append(Hash.get_Celda_Char(celdas_camino[i + 1]));

            if (camino.ToString().Length >= 3)
            {
                for (int i = 0; i <= camino.ToString().Length - 1; i += 3)
                {
                    if (!camino.ToString().get_Substring_Seguro(i, 1).Equals(camino.ToString().get_Substring_Seguro(i + 3, 1)))
                        pathfinding_limpio.Append(camino.ToString().get_Substring_Seguro(i, 3));
                }
            }
            else
            {
                pathfinding_limpio.Append(camino.ToString());
            }

            return pathfinding_limpio.ToString();
        }

        #region Zona Dispose
        ~Pathfinding() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                celdas = null;
                mapa = null;
                celdas_camino.Clear();
                celdas_camino = null;
                disposed = true;
            }
        }
        #endregion
    }
}
