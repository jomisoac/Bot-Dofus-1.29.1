using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;
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
        private Nodo[] celdas;
        private Mapa mapa { get; }
        private List<Nodo> lista_celdas_no_permitidas = new List<Nodo>();
        private List<Nodo> lista_celdas_permitidas = new List<Nodo>();
        private List<short> lista_celdas_camino = new List<short>();
        private Cuenta cuenta;
        private readonly int pm_pelea;
        public static int tiempo;
        private StringBuilder camino = new StringBuilder();
        private bool disposed;

        private static readonly Dictionary<TipoAnimacion, DuracionAnimacion> tiempo_tipo_animacion = new Dictionary<TipoAnimacion, DuracionAnimacion>()
        {
            { TipoAnimacion.MONTURA, new DuracionAnimacion(135, 200, 120) },
            { TipoAnimacion.CORRIENDO, new DuracionAnimacion(170, 255, 150) },
            { TipoAnimacion.CAMINANDO, new DuracionAnimacion(480, 510, 425) },
            { TipoAnimacion.FANTASMA, new DuracionAnimacion(57, 85, 50) }//speed hack ;)
        };

        public Pathfinding(Cuenta _cuenta, bool esquivar_monstruos, int _pm_pelea = 0)
        {
            cuenta = _cuenta;
            mapa = cuenta.personaje.mapa;
            celdas = new Nodo[mapa.celdas.Length];
            if (cuenta.esta_luchando())
                pm_pelea = _pm_pelea;
            
            rellenar_cuadricula();
            cargar_Obstaculos(esquivar_monstruos);
        }

        private void rellenar_cuadricula()
        {
            foreach (Celda celda in mapa.celdas)
                celdas[celda.id] = new Nodo(celda.id, mapa.get_Celda_X_Coordenadas(celda.id), mapa.get_Celda_Y_Coordenadas(celda.id), celda.tipo != TipoCelda.NO_CAMINABLE && celda.tipo != TipoCelda.OBJETO_INTERACTIVO || celda.objeto_interactivo_id != -1);
        }

        private void cargar_Obstaculos(bool esquivar_monstruos)
        {
            foreach (Celda celda in mapa.celdas)
            {
                if (celda.tipo == TipoCelda.CELDA_TELEPORT)
                    lista_celdas_no_permitidas.Add(celdas[celda.id]);

                else if (celda.objeto_interactivo_id != -1)
                {
                    if (celda.objeto_interactivo != null && !celda.objeto_interactivo.modelo.caminable)
                        lista_celdas_no_permitidas.Add(celdas[celda.id]);
                }
            }

            if (esquivar_monstruos && !cuenta.esta_luchando())
            {
                foreach (Monstruo monstruo in mapa.get_Monstruos().Values)
                    lista_celdas_no_permitidas.Add(celdas[monstruo.celda_id]);
            }

            if(cuenta.esta_luchando())
            {
                foreach (short celda in cuenta.pelea.get_Celdas_Ocupadas)
                    lista_celdas_no_permitidas.Add(celdas[celda]);
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

                    if (cuenta.esta_luchando()) continue;

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
                    if (lista_celdas_no_permitidas.Contains(celda_siguiente) || !celda_siguiente.es_caminable)
                        continue;

                    int temporal_g = actual.coste_g + get_Distancia_Nodos(celda_siguiente, actual);

                    if (!lista_celdas_permitidas.Contains(celda_siguiente))
                        lista_celdas_permitidas.Add(celda_siguiente);
                    else if (temporal_g > celda_siguiente.coste_g)
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
                if (cuenta.esta_luchando())
                {
                    if (get_Distancia_Nodos(nodo_actual, nodo_inicial) <= pm_pelea || get_Distancia_Nodos(nodo_inicial, nodo_final) <= pm_pelea)
                        lista_celdas_camino.Add(nodo_actual.id);
                }
                else
                    lista_celdas_camino.Add(nodo_actual.id);

                nodo_actual = nodo_actual.nodo_padre;
            }
            lista_celdas_camino.Add(nodo_inicial.id);
            lista_celdas_camino.Reverse();

            short celda_actual, celda_siguiente = 0;
            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                celda_actual = lista_celdas_camino[i];
                celda_siguiente = lista_celdas_camino[i + 1];
                camino.Append(get_Direccion_String(get_Orientacion_Casilla(celda_actual, celda_siguiente, mapa))).Append(get_Celda_Char(celda_siguiente));
            }
            cuenta.personaje.evento_Personaje_Pathfinding_Minimapa(lista_celdas_camino);
        }

        private List<Nodo> get_Celda_Siguiente(Nodo nodo)
        {
            List<Nodo> celdas_siguientes = new List<Nodo>();

            Nodo celda_derecha = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x + 1 && nodec.posicion_y == nodo.posicion_y);
            Nodo celda_izquierda = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x - 1 && nodec.posicion_y == nodo.posicion_y);
            Nodo celda_inferior = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x && nodec.posicion_y == nodo.posicion_y + 1);
            Nodo celda_superior = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x && nodec.posicion_y == nodo.posicion_y - 1);

            if (celda_derecha != null)
                celdas_siguientes.Add(celda_derecha);
            if (celda_izquierda != null)
                celdas_siguientes.Add(celda_izquierda);
            if (celda_inferior != null)
                celdas_siguientes.Add(celda_inferior);
            if (celda_superior != null)
                celdas_siguientes.Add(celda_superior);

            if (!cuenta.esta_luchando())//Diagonales
            {
                Nodo celda_superior_izquierda = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x - 1 && nodec.posicion_y == nodo.posicion_y - 1);
                Nodo celda_inferior_derecha = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x + 1 && nodec.posicion_y == nodo.posicion_y + 1);
                Nodo celda_inferior_izquierda = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x - 1 && nodec.posicion_y == nodo.posicion_y + 1);
                Nodo celda_superior_derecha = celdas.FirstOrDefault(nodec => nodec.posicion_x == nodo.posicion_x + 1 && nodec.posicion_y == nodo.posicion_y - 1);

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

        public int get_Tiempo_Desplazamiento_Mapa(int casilla_inicio, int casilla_final)
        {
            tiempo = 20;
            DuracionAnimacion tipo_animacion = lista_celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda anterior_celda = cuenta.personaje.mapa.celdas[casilla_inicio], siguiente_celda;

            for (int i = 0; i < lista_celdas_camino.Count - 1; i++)
            {
                siguiente_celda = cuenta.personaje.mapa.celdas[lista_celdas_camino[i + 1]];

                if (mapa.get_Celda_Y_Coordenadas(anterior_celda.id) == mapa.get_Celda_Y_Coordenadas(siguiente_celda.id))
                    tiempo += tipo_animacion.horizontal;
                else if (mapa.get_Celda_X_Coordenadas(anterior_celda.id) == mapa.get_Celda_Y_Coordenadas(siguiente_celda.id))
                    tiempo += tipo_animacion.vertical;
                else
                    tiempo += tipo_animacion.lineal;

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

        public static byte get_Orientacion_Casilla(short celda_1, short celda_2, Mapa mapa)
        {
            byte mapa_anchura = mapa.anchura;
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

        public static string get_Celda_Char(short celda)
        {
            int CharCode2 = celda % Hash.caracteres_array.Length;
            int CharCode1 = (celda - CharCode2) / Hash.caracteres_array.Length;
            return Hash.caracteres_array[CharCode1].ToString() + Hash.caracteres_array[CharCode2].ToString();
        }

        public static short get_Celda_Numero(int total_celdas, string celda_char)
        {
            for (short i = 0; i < total_celdas; i++)
            {
                if (get_Celda_Char(i) == celda_char)
                    return i;
            }
            return -1;
        }

        private int get_Distancia_Nodos(Nodo a, Nodo b)
        {
            if (cuenta.esta_luchando())
                return Math.Abs(a.posicion_x - b.posicion_x) + Math.Abs(a.posicion_y - b.posicion_y);

            return ((a.posicion_x - b.posicion_x) * (a.posicion_x - b.posicion_x)) + ((a.posicion_y - b.posicion_y) * (a.posicion_y - b.posicion_y));
        }

        public string get_Pathfinding_Limpio()
        {
            StringBuilder pathfinding_limpio = new StringBuilder();

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
