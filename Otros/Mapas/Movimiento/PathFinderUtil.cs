using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
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
    internal class PathFinderUtil
    {
        private static readonly Dictionary<TipoAnimacion, DuracionAnimacion> tiempo_tipo_animacion = new Dictionary<TipoAnimacion, DuracionAnimacion>()
        {
            { TipoAnimacion.MONTURA, new DuracionAnimacion(135, 200, 120) },
            { TipoAnimacion.CORRIENDO, new DuracionAnimacion(170, 255, 150) },
            { TipoAnimacion.CAMINANDO, new DuracionAnimacion(480, 510, 425) },
            { TipoAnimacion.FANTASMA, new DuracionAnimacion(57, 85, 50) }//speed hack ;)
        };

        public static int get_Tiempo_Desplazamiento_Mapa(Celda celda_actual, List<Celda> celdas_camino, Mapa _mapa)
        {
            int tiempo_desplazamiento = 20;
            DuracionAnimacion tipo_animacion = celdas_camino.Count < 6 ? tiempo_tipo_animacion[TipoAnimacion.CAMINANDO] : tiempo_tipo_animacion[TipoAnimacion.CORRIENDO];
            Celda siguiente_celda;

            for (int i = 0; i < celdas_camino.Count - 1; i++)
            {
                siguiente_celda = celdas_camino[i + 1];

                if (celda_actual.y == siguiente_celda.y)
                    tiempo_desplazamiento += tipo_animacion.horizontal;
                else if (celda_actual.x == siguiente_celda.y)
                    tiempo_desplazamiento += tipo_animacion.vertical;
                else
                    tiempo_desplazamiento += tipo_animacion.lineal;

                if (celda_actual.layer_ground_nivel < siguiente_celda.layer_ground_nivel)
                    tiempo_desplazamiento += 100;
                else if (siguiente_celda.layer_ground_nivel > celda_actual.layer_ground_nivel)
                    tiempo_desplazamiento -= 100;
                else if (celda_actual.layer_ground_slope != siguiente_celda.layer_ground_slope)
                {
                    if (celda_actual.layer_ground_slope == 1)
                        tiempo_desplazamiento += 100;
                    else if (siguiente_celda.layer_ground_slope == 1)
                        tiempo_desplazamiento -= 100;
                }
                celda_actual = siguiente_celda;
            }

            return tiempo_desplazamiento;
        }

        public static string get_Pathfinding_Limpio(List<Celda> camino)
        {
            Celda celda_destino = camino.Last();

            if (camino.Count <= 2)
                return celda_destino.get_Direccion(camino.First()) + Hash.get_Celda_Char(celda_destino.id);

            StringBuilder pathfinder = new StringBuilder();
            char direccion = camino[1].get_Direccion(camino.First());//primera direccion
            
            for (int i = 1; i < camino.Count - 1; i++)
            {
                Celda celda_actual = camino[i];
                Celda celda_siguiente = camino[i + 1];

                if (direccion != celda_siguiente.get_Direccion(celda_actual))
                {
                    pathfinder.Append(direccion);
                    pathfinder.Append(Hash.get_Celda_Char(celda_actual.id));

                    direccion = celda_siguiente.get_Direccion(celda_actual);
                }
            }

            pathfinder.Append(direccion);
            pathfinder.Append(Hash.get_Celda_Char(celda_destino.id));

            return pathfinder.ToString();
        }
    }
}
