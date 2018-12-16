using System;
using System.Collections.Generic;
using System.Text;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    internal class Pathfinding
    {
        public List<int> lista_abierta = new List<int>();
        public List<int> lista_cerrada = new List<int>();
        private readonly int[] Plist = new int[1025];
        private readonly int[] Flist = new int[1025];
        private readonly int[] Glist = new int[1025];
        private readonly int[] Hlist = new int[1025];
        public const string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
        private Mapa mapa;

        private bool es_pelea;
        private int nombreDePM;

        public Pathfinding(Mapa _mapa)
        {
            mapa = _mapa;
        }

        public static string get_Direccion_Char(int direccion)
        {
            if (direccion >= hash.Length)
                return "";
            return hash[direccion].ToString();
        }

        private void loadSprites()
        {
            for (int i = 0; i < (mapa.celdas.Length - 1); i++)
            {
                if (mapa.celdas[i].tipo_caminable < 4)
                {
                    lista_cerrada.Add(i);
                }
                else if (mapa.celdas[i].object2Movement)
                {
                    lista_cerrada.Add(i);
                }
            }
        }

        public string pathing(int celda_actual, int celda_final)
        {
            try
            {
                loadSprites();
                lista_cerrada.Remove(celda_final);
                return get_Pathfinding_Limpio(get_Pathfinding(celda_actual, celda_final));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string pathing(int celda_actual, int celda_final, bool _es_pelea, int _nombre_pm)
        {
            try
            {
                loadSprites();
                lista_cerrada.Remove(celda_final);

                es_pelea = _es_pelea;
                nombreDePM = _nombre_pm;
                return get_Pathfinding_Limpio(get_Pathfinding(celda_actual, celda_final));
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string get_Pathfinding(int celda_1, int celda_2)
        {
            int actual;
            int i = 0;

            lista_abierta.Add(celda_1);

            while (!lista_abierta.Contains(celda_2))
            {
                i += 1;
                if (i > 1000)
                    return "";

                actual = get_F_Punto();
                if (actual != celda_2)
                {
                    lista_cerrada.Add(actual);
                    lista_abierta.Remove(actual);

                    get_Hijo(actual).ForEach(celda =>
                    {
                        if (!lista_cerrada.Contains(celda))
                        {
                            if (lista_abierta.Contains(celda))
                            {
                                if (Glist[actual] + 5 < Glist[celda])
                                {
                                    Plist[celda] = actual;
                                    Glist[celda] = Glist[actual] + 5;
                                    Hlist[celda] = get_Distancia_Estimada(celda, celda_2);
                                    Flist[celda] = Glist[celda] + Hlist[celda];
                                }
                            }
                            else
                            {
                                lista_abierta.Add(celda);
                                lista_abierta[lista_abierta.Count - 1] = celda;
                                Glist[celda] = Glist[actual] + 5;
                                Hlist[celda] = get_Distancia_Estimada(celda, celda_2);
                                Flist[celda] = Glist[celda] + Hlist[celda];
                                Plist[celda] = actual;
                            }
                        }
                    });
                }
            }
            return get_Padre(celda_1, celda_2);
        }

        private string get_Padre(int cell1, int cell2)
        {
            int current = cell2;
            List<int> pathCell = new List<int>();
            pathCell.Add(current);

            while (current != cell1)
            {
                pathCell.Add(Plist[current]);
                current = Plist[current];
            }
            return getPath(pathCell);
        }

        private string getPath(List<int> camino_celda)
        {
            camino_celda.Reverse();
            StringBuilder pathing = new StringBuilder();
            int actual, hijo, pm_usados = 0;
            for (int i = 0; i < camino_celda.Count - 1; i++)
            {
                pm_usados += 1;
                if (pm_usados > nombreDePM && es_pelea)
                    return pathing.ToString();
                actual = camino_celda[i];
                hijo = camino_celda[i + 1];
                pathing.Append(get_Direccion_Char(get_Direccion_Casilla(actual, hijo))).Append(get_Celda_Chars(hijo));
            }
            return pathing.ToString();
        }

        public static string get_Celda_Chars(int celda)
        {
            int CharCode2 = celda % hash.Length;
            int CharCode1 = (celda - CharCode2) / hash.Length;
            return hash[CharCode1].ToString() + hash[CharCode2].ToString();
        }

        private List<int> get_Hijo(int celda_id)
        {
            int x = get_Celda_X_Coordenadas(celda_id), y = get_Celda_Y_Coordenadas(celda_id);
            int temporal, x_temporal, y_temporal;
            List<int> lista_hijo = new List<int>();

            if (!es_pelea)
            {
                temporal = celda_id - 29;
                x_temporal = get_Celda_X_Coordenadas(temporal);
                y_temporal = get_Celda_Y_Coordenadas(temporal);
                if (temporal > 1 & temporal < 1024 & x_temporal == x - 1 & y_temporal == y - 1 & !lista_cerrada.Contains(temporal))
                    lista_hijo.Add(temporal);

                temporal = celda_id + 29;
                x_temporal = get_Celda_X_Coordenadas(temporal);
                y_temporal = get_Celda_Y_Coordenadas(temporal);
                if (temporal > 1 & temporal < 1024 & x_temporal == x + 1 & y_temporal == y + 1 & !lista_cerrada.Contains(temporal))
                    lista_hijo.Add(temporal);
            }

            temporal = celda_id - 15;
            x_temporal = get_Celda_X_Coordenadas(temporal);
            y_temporal = get_Celda_Y_Coordenadas(temporal);
            if (temporal > 1 & temporal < 1024 & x_temporal == x - 1 & y_temporal == y & !lista_cerrada.Contains(temporal))
                lista_hijo.Add(temporal);

            temporal = celda_id + 15;
            x_temporal = get_Celda_X_Coordenadas(temporal);
            y_temporal = get_Celda_Y_Coordenadas(temporal);
            if (temporal > 1 & temporal < 1024 & x_temporal == x + 1 & y_temporal == y & !lista_cerrada.Contains(temporal))
                lista_hijo.Add(temporal);

            temporal = celda_id - 14;
            x_temporal = get_Celda_X_Coordenadas(temporal);
            y_temporal = get_Celda_Y_Coordenadas(temporal);
            if (temporal > 1 & temporal < 1024 & x_temporal == x & y_temporal == y - 1 & !lista_cerrada.Contains(temporal))
                lista_hijo.Add(temporal);

            temporal = celda_id + 14;
            x_temporal = get_Celda_X_Coordenadas(temporal);
            y_temporal = get_Celda_Y_Coordenadas(temporal);
            if (temporal > 1 & temporal < 1024 & x_temporal == x & y_temporal == y + 1 & !lista_cerrada.Contains(temporal))
                lista_hijo.Add(temporal);

            if (!es_pelea)
            {
                temporal = celda_id - 1;
                x_temporal = get_Celda_X_Coordenadas(temporal);
                y_temporal = get_Celda_Y_Coordenadas(temporal);
                if (temporal > 1 & temporal < 1024 & x_temporal == x - 1 & y_temporal == y + 1 & !lista_cerrada.Contains(temporal))
                    lista_hijo.Add(temporal);

                temporal = celda_id + 1;
                x_temporal = get_Celda_X_Coordenadas(temporal);
                y_temporal = get_Celda_Y_Coordenadas(temporal);
                if (temporal > 1 & temporal < 1024 & x_temporal == x + 1 & y_temporal == y - 1 & !lista_cerrada.Contains(temporal))
                    lista_hijo.Add(temporal);
            }
            return lista_hijo;
        }

        private int get_F_Punto()
        {
            int x = 9999;
            int cell = 0;

            foreach (int item in lista_abierta)
            {
                if (!lista_cerrada.Contains(item))
                {
                    if (Flist[item] < x)
                    {
                        x = Flist[item];
                        cell = item;
                    }
                }
            }
            return cell;
        }

        public int get_Celda_Y_Coordenadas(int celda_id)
        {
            int loc5 = celda_id / ((mapa.anchura * 2) - 1);
            int loc6 = celda_id - (loc5 * ((mapa.anchura * 2) - 1));
            int loc7 = loc6 % mapa.anchura;
            return loc5 - loc7;
        }

        public int get_Celda_X_Coordenadas(int celda_id) => (celda_id - ((mapa.anchura - 1) * get_Celda_Y_Coordenadas(celda_id))) / mapa.anchura;

        public int get_Distancia_Estimada(int id_1, int id_2)
        {
            if (id_1 == id_2)
                return 0;

            int diferencia_x = Math.Abs(get_Celda_X_Coordenadas(id_1) - get_Celda_X_Coordenadas(id_2));
            int diferencia_y = Math.Abs(get_Celda_Y_Coordenadas(id_1) - get_Celda_Y_Coordenadas(id_2));
            return diferencia_x + diferencia_y;
        }


        public int get_Direccion_Casilla(int celda_1, int celda_2)
        {
            int mapa_anchura = mapa.anchura;
            int[] _loc6_ = { 1, mapa_anchura, (mapa_anchura * 2) - 1, mapa_anchura - 1, -1, -mapa_anchura, (-mapa_anchura * 2) + 1, -(mapa_anchura - 1) };
            int _loc7_ = celda_2 - celda_1;

            for (int i = 7; i >= 0; i += -1)
            {
                if (_loc6_[i] == _loc7_)
                    return i;
            }

            int resultado_x = get_Celda_X_Coordenadas(celda_2) - get_Celda_X_Coordenadas(celda_1);
            int resultado_y = get_Celda_Y_Coordenadas(celda_2) - get_Celda_Y_Coordenadas(celda_1);

            if (resultado_x == 0)
            {
                if (resultado_y > 0)
                    return 3;
                return 7;
            }
            else if (resultado_x > 0)
            {
                return 1;
            }
            else
            {
                return 5;
            }
        }

        private string get_Pathfinding_Limpio(string pathfinding)
        {
            StringBuilder pathfinding_limpio = new StringBuilder();

            if (pathfinding.Length >= 3)
            {
                for (int i = 0; i <= pathfinding.Length - 1; i += 3)
                {
                    if (!pathfinding.get_Substring_Seguro(i, 1).Equals(pathfinding.get_Substring_Seguro(i + 3, 1)))
                    {
                        pathfinding_limpio.Append(pathfinding.get_Substring_Seguro(i, 3));
                    }
                }
            }
            else
            {
                pathfinding_limpio.Append(pathfinding);
            }
            return pathfinding_limpio.ToString();
        }
    }
}