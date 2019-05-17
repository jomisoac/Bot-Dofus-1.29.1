/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/
namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento
{
    public class NodoCeldas
    {
        public Celda celda { get; set; }
        public int coste_h { get; set; }
        public int coste_g { get; set; }
        public int coste_f { get; set; }
        public NodoCeldas nodo_padre { get; set; }

        public NodoCeldas(Celda _celda)
        {
            celda = _celda;
        }
    }
}
