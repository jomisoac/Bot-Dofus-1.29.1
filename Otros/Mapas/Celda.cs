/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using System;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public short id { get; private set; } = 0;
        public short objeto_interactivo_id { get; private set; } = 0;
        public TipoCelda tipo { get; private set; } = TipoCelda.NO_CAMINABLE;
        public bool es_linea_vision { get; private set; } = false;
        public byte layerGroundLevel { get; private set; } = 0;
        public byte layerGroundSlope { get; private set; } = 0;
        public ObjetoInteractivo objeto_interactivo { get; private set; }

        public Celda(short _id, TipoCelda _tipo, bool _es_linea_vision, byte _nivel, byte _slope, short _objeto_interactivo_id)
        {
            id = _id;
            tipo = _tipo;
            es_linea_vision = _es_linea_vision;
            layerGroundLevel = _nivel;
            layerGroundSlope = _slope;
            objeto_interactivo_id = _objeto_interactivo_id;

            if (objeto_interactivo_id != -1)
                objeto_interactivo = new ObjetoInteractivo(objeto_interactivo_id);
        }
    }
}
