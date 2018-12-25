using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot_Dofus_1._29._1.Controles.ControlMapa;
using Bot_Dofus_1._29._1.Interfaces;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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
        private StringBuilder camino = new StringBuilder();
        bool disposed;

        //Velocidades para esperar al enviar el GKK0
        public static double[] velocidad_corriendo = { 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001, 1.700000E-001, 1.500000E-001, 1.500000E-001, 1.500000E-001 };
        public static double[] velocidad_paseando = { 10E-2, 9E-2, 10E-2, 10E-2, 9E-2, 10E-2, 10E-2, 10E-2 };
        public static double[] velocidad_con_montura = { 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001, 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001 };

        public Pathfinding(Cuenta _cuenta, bool _es_pelea, bool esquivar_monstruos)
        {
            cuenta = _cuenta;
            mapa = cuenta.personaje.mapa;
            celdas = new Nodo[mapa.celdas.Length];
            es_pelea = _es_pelea;
            rellenar_cuadricula();
            cargar_Obstaculos(esquivar_monstruos);
        }

        private void rellenar_cuadricula()
        {
            Celda celda;
            for (int i = 0; i < mapa.celdas.Length; i++)
            {
                celda = mapa.celdas[i];
                celdas[i] = new Nodo(i, mapa.get_Celda_X_Coordenadas(i), mapa.get_Celda_Y_Coordenadas(i), celda.tipo != TipoCelda.NO_CAMINABLE && celda.tipo != TipoCelda.OBJETO_INTERACTIVO && !celda.object2Movement);
            }
        }

        private void cargar_Obstaculos(bool esquivar_monstruos)
        {
            if(esquivar_monstruos)
            {
                mapa.get_Monstruos().ToList().ForEach(x =>
                {
                    x.Value.ForEach(monstruo =>
                    {
                        if (monstruo.es_agresivo())
                        {
                            get_Celda_Siguiente(celdas[monstruo.celda_id], true).ForEach(celda_monstruo =>
                            {
                                lista_celdas_no_permitidas.Add(celdas[celda_monstruo.id]);
                            });
                        }
                    });
                });
            }
        }

        public bool get_Camino(int celda_inicio, int celda_final)
        {
            Nodo inicio = celdas[celda_inicio];
            Nodo final = celdas[celda_final];
            bool en_linea = get_Esta_En_Linea(celda_inicio, final.id);
            lista_celdas_permitidas.Add(inicio);

            while (lista_celdas_permitidas.Count > 0)
            {
                int index = 0;
                for (int i = 1; i < lista_celdas_permitidas.Count; i++)
                {
                    en_linea = get_Esta_En_Linea(lista_celdas_permitidas[i].id, lista_celdas_permitidas[index].id);

                    if (lista_celdas_permitidas[i].coste_f < lista_celdas_permitidas[index].coste_f)
                        index = i;

                    if (lista_celdas_permitidas[i].coste_f != lista_celdas_permitidas[index].coste_f) continue;
                    if (lista_celdas_permitidas[i].coste_g > lista_celdas_permitidas[index].coste_g)
                        index = i;

                    if (lista_celdas_permitidas[i].coste_g == lista_celdas_permitidas[index].coste_g)
                        index = i;

                    if (en_linea) continue;
                    
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
                
                foreach (Nodo celda_siguiente in get_Celda_Siguiente(actual, en_linea))
                {
                    if (lista_celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.es_caminable) continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual, en_linea);
                    if (!lista_celdas_permitidas.Contains(celda_siguiente))
                    {
                        lista_celdas_permitidas.Add(celda_siguiente);
                    }
                    else if (temporal_g >= celda_siguiente.coste_g)
                        continue;

                    celda_siguiente.coste_g = temporal_g;
                    celda_siguiente.coste_h = get_Distancia_Nodos(celda_siguiente, final, en_linea);
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

            int celda_actual, celda_siguiente = 0;
            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                celda_actual = lista_celdas_camino[i];
                celda_siguiente = lista_celdas_camino[i + 1];
                camino.Append(get_Direccion_Char(get_Orientacion_Casilla(celda_actual, celda_siguiente))).Append(get_Celda_Char(celda_siguiente));
            }
            cuenta.personaje.evento_Personaje_Pathfinding(lista_celdas_camino);
        }

        private List<Nodo> get_Celda_Siguiente(Nodo node, bool utiliar_diagonales)
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
            if (!utiliar_diagonales)
                return celdas_siguientes;

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

        public static string get_Direccion_Char(int direccion)
        {
            if (direccion >= Hash.caracteres_array.Length)
                return string.Empty;
            return Hash.caracteres_array[direccion].ToString();
        }
        
        public bool get_Esta_En_Linea(int celda_1, int celda_2)
        {
            bool x = mapa.get_Celda_X_Coordenadas(celda_1) == mapa.get_Celda_X_Coordenadas(celda_2);
            bool y = mapa.get_Celda_Y_Coordenadas(celda_1) == mapa.get_Celda_Y_Coordenadas(celda_2);

            return x || y;
        }

        public int get_Orientacion_Casilla(int celda_1, int celda_2)
        {
            int mapa_anchura = mapa.anchura;
            int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
            int _loc7_ = celda_2 - celda_1;

            for (int i = 7; i >= 0; i += -1)
            {
                if (_loc6_[i] == _loc7_)
                    return i;
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

        public double get_Tiempo_Desplazamiento_Mapa(int casilla_inicio, int casilla_final)
        {
            int distancia = mapa.get_Distancia_Entre_Dos_Casillas(casilla_inicio, casilla_final);
            int orientacion = get_Orientacion_Casilla(casilla_inicio, casilla_final);
            double tiempo;

            tiempo = (distancia < 6 ? velocidad_paseando[orientacion] : velocidad_corriendo[orientacion]) * 2100 * distancia;
            Celda primera_celda = cuenta.personaje.mapa.celdas[casilla_inicio], siguiente_celda;
            byte anterior_GroundLevel = primera_celda.layerGroundLevel;
            byte anterior_GroundSlope = primera_celda.layerGroundSlope;

            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                siguiente_celda = cuenta.personaje.mapa.celdas[lista_celdas_camino[i + 1]];

                if (anterior_GroundLevel < siguiente_celda.layerGroundLevel)
                    tiempo += 100;
                else if (siguiente_celda.layerGroundLevel > anterior_GroundLevel)
                    tiempo -= 100;
                else if (anterior_GroundSlope != siguiente_celda.layerGroundSlope)
                {
                    if (anterior_GroundSlope == 1)
                        tiempo += 100;
                    else if (siguiente_celda.layerGroundSlope == 1)
                        tiempo -= 100;
                }
            }

            return tiempo;
        }

        public int get_Tiempo_Desplazamiento_Pelea(int casilla_inicio, int casilla_final, Direcciones orientacion)
        {
            int distancia = mapa.get_Distancia_Entre_Dos_Casillas(casilla_inicio, casilla_final);
            switch (orientacion)
            {
                case Direcciones.ESTE:
                case Direcciones.OESTE:
                    return Math.Abs(casilla_inicio - casilla_final) * Convert.ToInt32(distancia >= 4 ? 875d / 2.5d : 875d);

                case Direcciones.NORTE:
                case Direcciones.SUR:
                    return Math.Abs(casilla_inicio - casilla_final) / ((mapa.anchura * 2) - 1) * Convert.ToInt32(distancia >= 4 ? 875d / 2.5d : 875d);

                case Direcciones.NORDESTE:
                case Direcciones.SUDESTE:
                    return Math.Abs(casilla_inicio - casilla_final) / (mapa.anchura - 1) * Convert.ToInt32(distancia >= 4 ? 625d / 2.5d : 625d);


                case Direcciones.NOROESTE:
                case Direcciones.SUDOESTE:
                    return Math.Abs(casilla_inicio - casilla_final) / (mapa.anchura - 1) * Convert.ToInt32(distancia >= 4 ? 625d / 2.5d : 625d);
            }
            return 0;
        }

        private int get_Distancia_Nodos(Nodo a, Nodo b, bool usar_diagonal)
        {
            if (usar_diagonal)
                return (int)Math.Sqrt((a.posicion_x - b.posicion_x) * (a.posicion_x - b.posicion_x) + (a.posicion_y - b.posicion_y) * (a.posicion_y - b.posicion_y));
            return Math.Abs(a.posicion_x - b.posicion_x) + Math.Abs(a.posicion_y - b.posicion_y);
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
