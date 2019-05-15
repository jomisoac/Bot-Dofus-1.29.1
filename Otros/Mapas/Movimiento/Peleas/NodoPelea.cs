﻿namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas
{
    public class NodoPelea
    {
        public short celda_id { get; private set; }
        public int pm_disponible { get; private set; }
        public int pa_disponible { get; private set; }
        public int distancia { get; private set; }

        public NodoPelea(short _celda_id, int _pm_disponible, int _pa_disponible, int _distancia)
        {
            celda_id = _celda_id;
            pm_disponible = _pm_disponible;
            pa_disponible = _pa_disponible;
            distancia = _distancia;
        }
    }
}
