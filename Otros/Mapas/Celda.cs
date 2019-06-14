/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using System;
using System.Linq;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public short id { get; private set; } = 0;
        public bool activa { get; private set; } = false;
        public TipoCelda tipo { get; private set; } = TipoCelda.NO_CAMINABLE;
        public bool es_linea_vision { get; private set; } = false;
        public byte layer_ground_nivel { get; private set; } = 0;
        public byte layer_ground_slope { get; private set; } = 0;
        public short layer_object_1_num { get; private set; } = 0;
        public short layer_object_2_num { get; private set; } = 0;
        public ObjetoInteractivo objeto_interactivo { get; private set; }
        public Mapa mapa { get; private set; } = null;
        public int x { get; private set; } = 0;
        public int y { get; private set; } = 0;

        /** pathfinder **/
        public int coste_h { get; set; } = 0;
        public int coste_g { get; set; } = 0;
        public int coste_f { get; set; } = 0;
        public Celda nodo_padre { get; set; } = null;

        public static readonly int[] texturas_teleport = { 1030, 1029, 1764, 2298, 745 };

        public Celda(short _id, bool esta_activa, TipoCelda _tipo, bool _es_linea_vision, byte _nivel, byte _slope, short _objeto_interactivo_id, short _layer_object_1_num, short _layer_object_2_num, Mapa _mapa)
        {
            id = _id;
            activa = esta_activa;
            tipo = _tipo;

            layer_object_1_num = _layer_object_1_num;
            layer_object_2_num = _layer_object_2_num;

            es_linea_vision = _es_linea_vision;
            layer_ground_nivel = _nivel;
            layer_ground_slope = _slope;
            mapa = _mapa;

            if (_objeto_interactivo_id != -1)
            {
                objeto_interactivo = new ObjetoInteractivo(_objeto_interactivo_id, this);
                mapa.interactivos.TryAdd(id, objeto_interactivo);
            }

            byte mapa_anchura = mapa.anchura;
            int _loc5 = id / ((mapa_anchura * 2) - 1);
            int _loc6 = id - (_loc5 * ((mapa_anchura * 2) - 1));
            int _loc7 = _loc6 % mapa_anchura;
            y = _loc5 - _loc7;
            x = (id - ((mapa_anchura - 1) * y)) / mapa_anchura;
        }

        public int get_Distancia_Entre_Dos_Casillas(Celda destino) => Math.Abs(x - destino.x) + Math.Abs(y - destino.y);
        
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

        public char get_Direccion(Celda celda)
        {
            if (x == celda.x)
                return celda.y < y ? (char)(3 + 'a') : (char)(7 + 'a');
            else if (y == celda.y)
                return celda.x < x ? (char)(1 + 'a') : (char)(5 + 'a');
            
            else if (x > celda.x)
                return y > celda.y ? (char)(2 + 'a') : (char)(0 + 'a');
            else if (x < celda.x)
                return y < celda.y ? (char)(6 + 'a') : (char)(4 + 'a');

            throw new Exception("Error direccion no encontrada");
        }

        public bool es_Teleport() => texturas_teleport.Contains(layer_object_1_num) || texturas_teleport.Contains(layer_object_2_num);
        public bool es_Interactivo() => tipo == TipoCelda.OBJETO_INTERACTIVO || objeto_interactivo != null;
        public bool es_Caminable() => activa && tipo != TipoCelda.NO_CAMINABLE && !es_Interactivo_Caminable();
        public bool es_Interactivo_Caminable() => tipo == TipoCelda.OBJETO_INTERACTIVO || objeto_interactivo != null && !objeto_interactivo.modelo.caminable;
    }
}
