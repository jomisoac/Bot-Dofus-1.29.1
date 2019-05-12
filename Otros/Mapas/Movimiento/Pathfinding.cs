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
        private Nodo[] celdas;
        private Mapa mapa { get; set; }
        private List<Nodo> lista_celdas_no_permitidas = new List<Nodo>();
        private List<Nodo> celdas_camino = new List<Nodo>();
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
                if (celda.tipo == TipoCelda.CELDA_TELEPORT && !cuenta.esta_luchando())
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

            List<Nodo> lista_celdas_permitidas = new List<Nodo>() { inicio };
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

                    if (!cuenta.esta_luchando()) continue;

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
                if (cuenta.esta_luchando())
                {
                    if (get_Distancia_Nodos(nodo_actual, nodo_inicial) <= pm_pelea || get_Distancia_Nodos(nodo_inicial, nodo_final) <= pm_pelea)
                        celdas_camino.Add(nodo_actual);
                }
                else
                    celdas_camino.Add(nodo_actual);

                nodo_actual = nodo_actual.nodo_padre;
            }

            celdas_camino.Add(nodo_inicial);
            celdas_camino.Reverse();
            
            for (int i = 0; i < celdas_camino.Count - 1; i++)
                camino.Append(get_Direccion_Dos_Celdas(celdas_camino[i], celdas_camino[i + 1])).Append(Hash.get_Celda_Char(celdas_camino[i + 1].id));

            cuenta.personaje.evento_Personaje_Pathfinding_Minimapa(celdas_camino);
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

        public int get_Tiempo_Desplazamiento_Mapa(int casilla_inicio, int casilla_final)
        {
            tiempo = 20;
            DuracionAnimacion tipo_animacion = celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda anterior_celda = cuenta.personaje.mapa.celdas[casilla_inicio], siguiente_celda;

            for (int i = 0; i < celdas_camino.Count - 1; i++)
            {
                siguiente_celda = cuenta.personaje.mapa.celdas[celdas_camino[i + 1].id];

                if (celdas_camino[i].posicion_y == celdas_camino[i + 1].posicion_y)
                    tiempo += tipo_animacion.horizontal;
                else if (celdas_camino[i].posicion_x == celdas_camino[i].posicion_y)
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

        private char get_Direccion_Dos_Celdas(Nodo celda_1, Nodo celda_2)
        {
            byte mapa_anchura = mapa.anchura;
            int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
            int _loc7_ = celda_2.id - celda_1.id;

            for (int i = 7; i >= 0; i += -1)
            {
                if (_loc6_[i] == _loc7_)
                    return (char)(i + 'a');
            }

            int resultado_x = celda_2.posicion_x - celda_1.posicion_x;
            int resultado_y = celda_2.posicion_y - celda_1.posicion_y;

            if (resultado_x == 0)
            {
                if (resultado_y > 0)
                    return (char) (3 + 'a');
                else
                    return (char)(7 + 'a');
            }
            else if (resultado_x > 0)
                return (char)(1 + 'a');
            else
                return (char)(5 + 'a');
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
                celdas_camino.Clear();
                celdas_camino = null;
                camino.Clear();
                camino = null;
                disposed = true;
            }
        }
        #endregion
    }
}
