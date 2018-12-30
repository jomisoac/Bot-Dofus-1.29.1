/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public int id { get; set; } = 0;
        public int objeto_interactivo_id { get; set; } = 0;
        public TipoCelda tipo { get; set; } = TipoCelda.NO_CAMINABLE;
        public bool es_linea_vision { get; set; } = false;
        public bool es_celda_interactiva { get; set; } = false;
        public byte layerGroundLevel { get; set; } = 0;
        public byte layerGroundSlope { get; set; } = 0;

        public Celda(int _id) => id = _id;
    }
}
