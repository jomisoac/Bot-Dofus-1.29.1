using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System.Collections.Generic;
using System.Text;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento
{
    class PathFinderUtil
    {
        private static readonly Dictionary<TipoAnimacion, DuracionAnimacion> tiempo_tipo_animacion = new Dictionary<TipoAnimacion, DuracionAnimacion>()
        {
            { TipoAnimacion.MONTURA, new DuracionAnimacion(135, 200, 120) },
            { TipoAnimacion.CORRIENDO, new DuracionAnimacion(170, 255, 150) },
            { TipoAnimacion.CAMINANDO, new DuracionAnimacion(480, 510, 425) },
            { TipoAnimacion.FANTASMA, new DuracionAnimacion(57, 85, 50) }//speed hack ;)
        };

        public static int get_Tiempo_Desplazamiento_Mapa(Celda casilla_actual, List<short> celdas_camino, Mapa _mapa)
        {
            int tiempo_desplazamiento = 20;
            DuracionAnimacion tipo_animacion = celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda siguiente_celda;

            for (int i = 0; i < celdas_camino.Count - 1; i++)
            {
                siguiente_celda = _mapa.celdas[celdas_camino[i + 1]];

                if (casilla_actual.y == siguiente_celda.y)
                    tiempo_desplazamiento += tipo_animacion.horizontal;
                else if (casilla_actual.x == siguiente_celda.y)
                    tiempo_desplazamiento += tipo_animacion.vertical;
                else
                    tiempo_desplazamiento += tipo_animacion.lineal;

                if (casilla_actual.layer_ground_nivel < siguiente_celda.layer_ground_nivel)
                    tiempo_desplazamiento += 100;
                else if (siguiente_celda.layer_ground_nivel > casilla_actual.layer_ground_nivel)
                    tiempo_desplazamiento -= 100;
                else if (casilla_actual.layer_ground_slope != siguiente_celda.layer_ground_slope)
                {
                    if (casilla_actual.layer_ground_slope == 1)
                        tiempo_desplazamiento += 100;
                    else if (siguiente_celda.layer_ground_slope == 1)
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

            if (!es_pelea)
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
    }
}
