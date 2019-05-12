/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;
using System.Linq;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public short id { get; private set; } = 0;
        public short objeto_interactivo_id { get; private set; } = 0;
        public TipoCelda tipo { get; private set; } = TipoCelda.NO_CAMINABLE;
        public bool es_linea_vision { get; private set; } = false;
        public byte layer_ground_nivel { get; private set; } = 0;
        public byte layer_ground_slope { get; private set; } = 0;
        public ObjetoInteractivo objeto_interactivo { get; private set; }
        public MapaTeleportCeldas tipo_teleport { get; private set; } = MapaTeleportCeldas.NINGUNO;

        public static readonly int[] texturas_teleport = { 1030, 1029, 1764, 2298, 745 };

        public Celda(short _id, TipoCelda _tipo, bool _es_linea_vision, byte _nivel, byte _slope, short _objeto_interactivo_id, short layer_object_1_num, short layer_object_2_num, Mapa _mapa)
        {
            id = _id;

            if (texturas_teleport.Contains(layer_object_1_num) || texturas_teleport.Contains(layer_object_2_num))
                tipo = TipoCelda.CELDA_TELEPORT;
            else if (_tipo == TipoCelda.CELDA_TELEPORT)
                tipo = TipoCelda.CELDA_CAMINABLE;
            else
                tipo = _tipo;

            es_linea_vision = _es_linea_vision;
            layer_ground_nivel = _nivel;
            layer_ground_slope = _slope;
            objeto_interactivo_id = _objeto_interactivo_id;

            if (objeto_interactivo_id != -1)
                objeto_interactivo = new ObjetoInteractivo(objeto_interactivo_id);

            if (tipo == TipoCelda.CELDA_TELEPORT)
            {
                int temporal_x = _mapa.get_Celda_X_Coordenadas(id), temporal_y = _mapa.get_Celda_Y_Coordenadas(id);

                if ((temporal_x - 1) == temporal_y)
                    tipo_teleport = MapaTeleportCeldas.IZQUIERDA;
                else if ((temporal_x - 27) == temporal_y)
                    tipo_teleport = MapaTeleportCeldas.DERECHA;
                else if ((temporal_x + temporal_y) == 31)
                    tipo_teleport = MapaTeleportCeldas.ABAJO;
                else if (temporal_y < 0)
                {
                    if ((temporal_x - Math.Abs(temporal_y)) == 1)
                        tipo_teleport = MapaTeleportCeldas.ARRIBA;
                }
            }
        }
    }
}
