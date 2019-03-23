using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Interfaces;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

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
        private Nodo[] celdas;
        private Mapa mapa { get; }
        private readonly bool es_pelea;
        private List<Nodo> lista_celdas_no_permitidas = new List<Nodo>();
        private List<Nodo> lista_celdas_permitidas = new List<Nodo>();
        private List<int> lista_celdas_camino = new List<int>();
        private Cuenta cuenta;
        private int pm_pelea;
        private StringBuilder camino = new StringBuilder();
        bool disposed;

        private static readonly Dictionary<TipoAnimacion, DuracionAnimacion> tiempo_tipo_animacion = new Dictionary<TipoAnimacion, DuracionAnimacion>()
        {
            { TipoAnimacion.MONTURA, new DuracionAnimacion(135, 200, 120) },
            { TipoAnimacion.CORRIENDO, new DuracionAnimacion(170, 255, 150) },
            { TipoAnimacion.CAMINANDO, new DuracionAnimacion(480, 510, 425) },
            { TipoAnimacion.FANTASMA, new DuracionAnimacion(57, 85, 50) }
        };

        public Pathfinding(Cuenta _cuenta, bool _es_pelea, bool esquivar_monstruos, int _pm_pelea = 0)
        {
            cuenta = _cuenta;
            mapa = cuenta.personaje.mapa;
            celdas = new Nodo[mapa.celdas.Length];
            es_pelea = _es_pelea;
            if(es_pelea)
                pm_pelea = _pm_pelea;
            rellenar_cuadricula();
            cargar_Obstaculos(esquivar_monstruos);
        }

        private void rellenar_cuadricula()
        {
            Celda celda;
            for (int i = 0; i < mapa.celdas.Length; i++)
            {
                celda = mapa.celdas[i];
                celdas[i] = new Nodo(i, mapa.get_Celda_X_Coordenadas(i), mapa.get_Celda_Y_Coordenadas(i), celda.tipo != TipoCelda.NO_CAMINABLE);
             }
        }

        private void cargar_Obstaculos(bool esquivar_monstruos)
        {
            foreach(Celda celda in mapa.celdas)
            {
                if (celda.tipo == TipoCelda.CELDA_TELEPORT)
                    lista_celdas_no_permitidas.Add(celdas[celda.id]);
                else if(celda.es_celda_interactiva)
                    lista_celdas_no_permitidas.Add(celdas[celda.id]);
            }

            if(esquivar_monstruos)
            {
                mapa.get_Monstruos().ToList().ForEach(monstruo =>
                {
                    if (monstruo.Value.es_agresivo())
                    {
                        get_Celda_Siguiente(celdas[monstruo.Value.celda_id]).ForEach(celda_monstruo =>
                        {
                            lista_celdas_no_permitidas.Add(celdas[celda_monstruo.id]);
                        });
                    }
                });
            }
        }

        public bool get_Camino(int celda_inicio, int celda_final)
        {
            Nodo inicio = celdas[celda_inicio];
            Nodo final = celdas[celda_final];
            lista_celdas_permitidas.Add(inicio);
            lista_celdas_no_permitidas.Remove(celdas[celda_final]);

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

                    if (es_pelea) continue;
                    
                    if (lista_celdas_permitidas[i].coste_g == lista_celdas_permitidas[index].coste_g)
                        index = i;
                }

                Nodo actual = lista_celdas_permitidas[index];
                if (actual == final)
                {
                    get_Camino_Retroceso(inicio, final);
                    return true;
                }
                lista_celdas_permitidas.Remove(actual);
                lista_celdas_no_permitidas.Add(actual);
                
                foreach (Nodo celda_siguiente in get_Celda_Siguiente(actual))
                {
                    if (lista_celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.es_caminable) continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);
                    if (!lista_celdas_permitidas.Contains(celda_siguiente))
                    {
                        lista_celdas_permitidas.Add(celda_siguiente);
                    }
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

        private void get_Camino_Retroceso(Nodo nodo_inicial, Nodo nodo_final)
        {
            Nodo nodo_actual = nodo_final;

            while (nodo_actual != nodo_inicial)
            {
                lista_celdas_camino.Add(nodo_actual.id);
                nodo_actual = nodo_actual.nodo_padre;
            }
            lista_celdas_camino.Add(nodo_inicial.id);
            lista_celdas_camino.Reverse();

            int pm_usados = 0, celda_actual, celda_siguiente = 0;
            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                if(es_pelea)
                {
                    pm_usados += 1;
                    if (pm_usados > pm_pelea)
                        break;
                }
                celda_actual = lista_celdas_camino[i];
                celda_siguiente = lista_celdas_camino[i + 1];
                camino.Append(get_Direccion_String(get_Orientacion_Casilla(celda_actual, celda_siguiente, mapa))).Append(get_Celda_Char(celda_siguiente));
            }
            cuenta.personaje.evento_Personaje_Pathfinding(lista_celdas_camino);
        }

        private List<Nodo> get_Celda_Siguiente(Nodo node)
        {
            List<Nodo> celdas_siguientes = new List<Nodo>();

            Nodo celda_derecha = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) + 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id));
            Nodo celda_izquierda = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) - 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id));
            Nodo celda_inferior = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) + 1);
            Nodo celda_superior = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) - 1);

            if (celda_derecha != null)
                celdas_siguientes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_siguientes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_siguientes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_siguientes.Add(celda_superior);

            if (!es_pelea)
            {
                //Diagonales
                Nodo celda_superior_izquierda = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) - 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) - 1);
                Nodo celda_inferior_derecha = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) + 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) + 1);
                Nodo celda_inferior_izquierda = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) - 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) + 1);
                Nodo celda_superior_derecha = celdas.FirstOrDefault(nodec => mapa.get_Celda_X_Coordenadas(nodec.id) == mapa.get_Celda_X_Coordenadas(node.id) + 1 && mapa.get_Celda_Y_Coordenadas(nodec.id) == mapa.get_Celda_Y_Coordenadas(node.id) - 1);

                if (celda_superior_izquierda != null)
                    celdas_siguientes.Add(celda_superior_izquierda);
                if (celda_inferior_derecha != null)
                    celdas_siguientes.Add(celda_inferior_derecha);
                if (celda_inferior_izquierda != null)
                    celdas_siguientes.Add(celda_inferior_izquierda);
                if (celda_superior_derecha != null)
                    celdas_siguientes.Add(celda_superior_derecha);
            }
            return celdas_siguientes;
        }

        public static string get_Direccion_String(int direccion)
        {
            if (direccion >= Hash.caracteres_array.Length)
                return string.Empty;
            return Hash.caracteres_array[direccion].ToString();
        }

        public static char get_Direccion_Char(int direccion)
        {
            if (direccion >= Hash.caracteres_array.Length)
                return char.MaxValue;
            return Hash.caracteres_array[direccion];
        }

        public static byte get_Orientacion_Casilla(int celda_1, int celda_2, Mapa mapa)
        {
            int mapa_anchura = mapa.anchura;
            int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
            int _loc7_ = celda_2 - celda_1;

            for (int i = 7; i >= 0; i += -1)
            {
                if (_loc6_[i] == _loc7_)
                    return Convert.ToByte(i);
            }

            int resultado_x = mapa.get_Celda_X_Coordenadas(celda_2) - mapa.get_Celda_X_Coordenadas(celda_1);
            int resultado_y = mapa.get_Celda_Y_Coordenadas(celda_2) - mapa.get_Celda_Y_Coordenadas(celda_1);

            if (resultado_x == 0)
            {
                if (resultado_y > 0)
                    return 3;
                else
                    return 7;
            }
            else if (resultado_x > 0)
                return 1;
            else
                return 5;
        }

        public static string get_Celda_Char(int celda)
        {
            int CharCode2 = celda % Hash.caracteres_array.Length;
            int CharCode1 = (celda - CharCode2) / Hash.caracteres_array.Length;
            return Hash.caracteres_array[CharCode1].ToString() + Hash.caracteres_array[CharCode2].ToString();
        }

        public static int get_Celda_Numero(int total_celdas, string celda_char)
        {
            for (int i = 0; i < total_celdas; i++)
            {
                if (get_Celda_Char(i) == celda_char)
                {
                    return i;
                }
            }
            return -1;
        }

        public int get_Tiempo_Desplazamiento_Mapa(int casilla_inicio, int casilla_final)
        {
            int tiempo = 20;
            DuracionAnimacion tipo_animacion = lista_celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda anterior_celda = cuenta.personaje.mapa.celdas[casilla_inicio], siguiente_celda;

            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                siguiente_celda = cuenta.personaje.mapa.celdas[lista_celdas_camino[i + 1]];

                if(mapa.get_Celda_Y_Coordenadas(anterior_celda.id) == mapa.get_Celda_Y_Coordenadas(siguiente_celda.id))
                    tiempo += tipo_animacion.horizontal;
                else if (mapa.get_Celda_X_Coordenadas(anterior_celda.id) == mapa.get_Celda_Y_Coordenadas(siguiente_celda.id))
                    tiempo += tipo_animacion.vertical;
                else
                {
                    if(!es_pelea)
                        tiempo += tipo_animacion.lineal;
                }
                   
                if (anterior_celda.layerGroundLevel < siguiente_celda.layerGroundLevel)
                    tiempo += 100;
                else if (siguiente_celda.layerGroundLevel > anterior_celda.layerGroundLevel)
                    tiempo -= 100;
                else if (anterior_celda.layerGroundSlope != siguiente_celda.layerGroundSlope)
                {
                    if (anterior_celda.layerGroundSlope == 1)
                        tiempo += 100;
                    else if (siguiente_celda.layerGroundSlope == 1)
                        tiempo -= 100;
                }
                anterior_celda = siguiente_celda;
            }
            return tiempo;
        }

        private int get_Distancia_Nodos(Nodo a, Nodo b)
        {
            if(es_pelea)
                return Math.Abs(a.posicion_x - b.posicion_x) + Math.Abs(a.posicion_y - b.posicion_y);
            return Convert.ToInt32(Math.Sqrt(Math.Pow(a.posicion_x - b.posicion_x, 2) + Math.Pow(a.posicion_y - b.posicion_y, 2)));
        }

        public string get_Pathfinding_Limpio()
        {
            StringBuilder pathfinding_limpio = new StringBuilder();

            if (camino.ToString().Length >= 3)
            {
                for (int i = 0; i <= camino.ToString().Length - 1; i += 3)
                {
                    if (!camino.ToString().get_Substring_Seguro(i, 1).Equals(camino.ToString().get_Substring_Seguro(i + 3, 1)))
                    {
                        pathfinding_limpio.Append(camino.ToString().get_Substring_Seguro(i, 3));
                    }
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
                cuenta = null;
                lista_celdas_no_permitidas.Clear();
                lista_celdas_no_permitidas = null;
                lista_celdas_permitidas.Clear();
                lista_celdas_permitidas = null;
                lista_celdas_camino.Clear();
                lista_celdas_camino = null;
                camino.Clear();
                camino = null;
                disposed = true;
            }
        }
        #endregion
    }
}
