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
        private NodoCeldas[] cuadricula_celdas { get; set; }
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
            celdas_camino = new List<short>();
            cuadricula_celdas = new NodoCeldas[mapa.celdas.Length];

            rellenar_cuadricula();
        }

        private void rellenar_cuadricula()
        {
            foreach (Celda celda in mapa.celdas)
                cuadricula_celdas[celda.id] = new NodoCeldas(celda);
        }

        private void cargar_Obstaculos(bool esquivar_monstruos, out List<NodoCeldas> lista_celdas_no_permitidas)
        {
            lista_celdas_no_permitidas = new List<NodoCeldas>();

            foreach (Celda celda in mapa.celdas)
            {
                if (celda.tipo == TipoCelda.CELDA_TELEPORT)
                    lista_celdas_no_permitidas.Add(cuadricula_celdas[celda.id]);
            }

            if (esquivar_monstruos)
            {
                foreach (Monstruo monstruo in mapa.get_Monstruos().Values)
                    lista_celdas_no_permitidas.Add(cuadricula_celdas[monstruo.celda.id]);
            }
        }

        public bool get_Puede_Caminar(int celda_inicio, int celda_final, bool esquivar_monstruos)
        {
            NodoCeldas inicio = cuadricula_celdas[celda_inicio];
            NodoCeldas final = cuadricula_celdas[celda_final];

            List<NodoCeldas> lista_celdas_permitidas = new List<NodoCeldas>() { inicio };
            List<NodoCeldas> lista_celdas_no_permitidas;

            cargar_Obstaculos(esquivar_monstruos, out lista_celdas_no_permitidas);
            lista_celdas_no_permitidas.Remove(cuadricula_celdas[celda_final]);

            while (lista_celdas_permitidas.Count > 0)
            {
                int index = 0;
                for (int i = 1; i < lista_celdas_permitidas.Count; i++)
                {
                    if (lista_celdas_permitidas[i].coste_f < lista_celdas_permitidas[index].coste_f)
                        index = i;

                    if (lista_celdas_permitidas[i].coste_f != lista_celdas_permitidas[index].coste_f) continue;
                    if (lista_celdas_permitidas[i].coste_g > lista_celdas_permitidas[index].coste_g)
                        index = i;

                    if (lista_celdas_permitidas[i].coste_g == lista_celdas_permitidas[index].coste_g)
                        index = i;

                    if (lista_celdas_permitidas[i].coste_g == lista_celdas_permitidas[index].coste_g)
                        index = i;
                }

                NodoCeldas actual = lista_celdas_permitidas[index];
                if (actual == final)
                {
                    get_Camino_Retroceso(inicio, final);
                    return true;
                }
                lista_celdas_permitidas.Remove(actual);
                lista_celdas_no_permitidas.Add(actual);

                foreach (NodoCeldas celda_siguiente in get_Celdas_Adyecentes(actual))
                {
                    if (lista_celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.celda.es_Caminable())
                        continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);

                    if (!lista_celdas_permitidas.Contains(celda_siguiente))
                        lista_celdas_permitidas.Add(celda_siguiente);
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

        private void get_Camino_Retroceso(NodoCeldas nodo_inicial, NodoCeldas nodo_final)
        {
            NodoCeldas nodo_actual = nodo_final;

            while (nodo_actual != nodo_inicial)
            {
                celdas_camino.Add(nodo_actual.celda.id);
                nodo_actual = nodo_actual.nodo_padre;
            }

            celdas_camino.Add(nodo_inicial.celda.id);
            celdas_camino.Reverse();
        }

        public List<NodoCeldas> get_Celdas_Adyecentes(NodoCeldas nodo)
        {
            List<NodoCeldas> celdas_siguientes = new List<NodoCeldas>();

            NodoCeldas celda_derecha = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x + 1 && nodec.celda.y == nodo.celda.y);
            NodoCeldas celda_izquierda = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x - 1 && nodec.celda.y == nodo.celda.y);
            NodoCeldas celda_inferior = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x && nodec.celda.y == nodo.celda.y + 1);
            NodoCeldas celda_superior = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x && nodec.celda.y == nodo.celda.y - 1);

            if (celda_derecha != null)
                celdas_siguientes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_siguientes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_siguientes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_siguientes.Add(celda_superior);
            
            NodoCeldas superior_izquierda = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x - 1 && nodec.celda.y == nodo.celda.y - 1);
            NodoCeldas inferior_derecha = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x + 1 && nodec.celda.y == nodo.celda.y + 1);
            NodoCeldas inferior_izquierda = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x - 1 && nodec.celda.y == nodo.celda.y + 1);
            NodoCeldas superior_derecha = cuadricula_celdas.FirstOrDefault(nodec => nodec.celda.x == nodo.celda.x + 1 && nodec.celda.y == nodo.celda.y - 1);

            if (superior_izquierda != null)
                celdas_siguientes.Add(superior_izquierda);
            if (inferior_derecha != null)
                celdas_siguientes.Add(inferior_derecha);
            if (inferior_izquierda != null)
                celdas_siguientes.Add(inferior_izquierda);
            if (superior_derecha != null)
                celdas_siguientes.Add(superior_derecha);

            return celdas_siguientes;
        }

        public int get_Tiempo_Desplazamiento_Mapa(int casilla_inicio, int casilla_final)
        {
            tiempo_desplazamiento = 20;
            DuracionAnimacion tipo_animacion = celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda anterior_celda = mapa.celdas[casilla_inicio], siguiente_celda;

            for (int i = 0; i < celdas_camino.Count - 1; i++)
            {
                siguiente_celda = mapa.celdas[celdas_camino[i + 1]];

                if (cuadricula_celdas[anterior_celda.id].celda.y == cuadricula_celdas[siguiente_celda.id].celda.y)
                    tiempo_desplazamiento += tipo_animacion.horizontal;
                else if (cuadricula_celdas[anterior_celda.id].celda.x == cuadricula_celdas[siguiente_celda.id].celda.y)
                    tiempo_desplazamiento += tipo_animacion.vertical;
                else
                    tiempo_desplazamiento += tipo_animacion.lineal;

                if (anterior_celda.layer_ground_nivel < siguiente_celda.layer_ground_nivel)
                    tiempo_desplazamiento += 100;
                else if (siguiente_celda.layer_ground_nivel > anterior_celda.layer_ground_nivel)
                    tiempo_desplazamiento -= 100;
                else if (anterior_celda.layer_ground_slope != siguiente_celda.layer_ground_slope)
                {
                    if (anterior_celda.layer_ground_slope == 1)
                        tiempo_desplazamiento += 100;
                    else if (siguiente_celda.layer_ground_slope == 1)
                        tiempo_desplazamiento -= 100;
                }
                anterior_celda = siguiente_celda;
            }

            return tiempo_desplazamiento;
        }

        private static char get_Direccion_Dos_Celdas(short celda_1, short celda_2, Mapa mapa)
        {
            Celda celda1 = mapa.celdas[celda_1], celda2 = mapa.celdas[celda_2];
            byte mapa_anchura = mapa.anchura;
            int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
            int _loc7_ = celda_2 - celda_1;

            for (int i = 7; i >= 0; i += -1)
            {
                if (_loc6_[i] == _loc7_)
                    return (char)(i + 'a');
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

        private int get_Distancia_Nodos(NodoCeldas a, NodoCeldas b) => ((a.celda.x - b.celda.x) * (a.celda.x - b.celda.x)) + ((a.celda.y - b.celda.y) * (a.celda.y - b.celda.y));

        public static string get_Pathfinding_Limpio(List<short> celdas_camino, Mapa mapa)
        {
            StringBuilder pathfinding_limpio = new StringBuilder(), camino = new StringBuilder();

            for (int i = 0; i < celdas_camino.Count - 1; i++)
                camino.Append(get_Direccion_Dos_Celdas(celdas_camino[i], celdas_camino[i + 1], mapa)).Append(Hash.get_Celda_Char(celdas_camino[i + 1]));

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
                cuadricula_celdas = null;
                mapa = null;
                celdas_camino.Clear();
                celdas_camino = null;
                disposed = true;
            }
        }
        #endregion
    }
}
