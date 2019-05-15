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
        public short id { get; set; }
        public int posicion_x { get; set; }
        public int posicion_y { get; set; }
        public bool es_caminable { get; set; }
        public int coste_h { get; set; }
        public int coste_g { get; set; }
        public int coste_f { get; set; }
        public NodoCeldas nodo_padre { get; set; }

        public NodoCeldas(short _id, int _posicion_x, int _posicion_y, bool _es_caminable)
        {
            id = _id;
            posicion_x = _posicion_x;
            posicion_y = _posicion_y;
            es_caminable = _es_caminable;
        }
    }
}
