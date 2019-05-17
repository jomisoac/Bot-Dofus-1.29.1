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
        public Mapa mapa { get; private set; } = null;
        public int x { get; private set; } = 0;
        public int y { get; private set; } = 0;

        /** pathfinder **/
        public int coste_h { get; set; } = 0;
        public int coste_g { get; set; } = 0;
        public int coste_f { get; set; } = 0;
        public Celda nodo_padre { get; set; } = null;

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
            mapa = _mapa;

            if (objeto_interactivo_id != -1)
                objeto_interactivo = new ObjetoInteractivo(objeto_interactivo_id);
            
            byte mapa_anchura = mapa.anchura;
            int _loc5 = id / ((mapa_anchura * 2) - 1);
            int _loc6 = id - (_loc5 * ((mapa_anchura * 2) - 1));
            int _loc7 = _loc6 % mapa_anchura;
            y = _loc5 - _loc7;
            x = (id - ((mapa_anchura - 1) * y)) / mapa_anchura;

            if (tipo == TipoCelda.CELDA_TELEPORT)
            {
                if ((x - 1) == y)
                    tipo_teleport = MapaTeleportCeldas.IZQUIERDA;
                else if ((x - 27) == y)
                    tipo_teleport = MapaTeleportCeldas.DERECHA;
                else if ((x + y) == 31)
                    tipo_teleport = MapaTeleportCeldas.ABAJO;
                else if (y < 0)
                {
                    if ((x - Math.Abs(y)) == 1)
                        tipo_teleport = MapaTeleportCeldas.ARRIBA;
                }
            }
        }

        public int get_Distancia_Entre_Dos_Casillas(short destino)
        {
            Celda celda_destino = mapa.celdas[destino];

            return Math.Abs(x - celda_destino.x) + Math.Abs(y - celda_destino.y);
        }

        public bool get_Esta_En_Linea(short destino)
        {
            Celda celda_destino = mapa.celdas[destino];

            return x == celda_destino.x || y == celda_destino.y;
        }

        public bool es_Caminable() => tipo != TipoCelda.NO_CAMINABLE && !es_Interactivo();

        public bool es_Interactivo()
        {
            if (tipo == TipoCelda.OBJETO_INTERACTIVO)
                return true;

            return objeto_interactivo != null && !objeto_interactivo.modelo.caminable;
        }
    }
}
