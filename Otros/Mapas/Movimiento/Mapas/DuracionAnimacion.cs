/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/
namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas
{
    internal class DuracionAnimacion
    {
        public int lineal { get; private set; }
        public int horizontal { get; private set; }
        public int vertical { get; private set; }

        public DuracionAnimacion(int _lineal, int _horizontal, int _vertical)
        {
            lineal = _lineal;
            horizontal = _horizontal;
            vertical = _vertical;
        }
    }
}
