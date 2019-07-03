using Bot_Dofus_1._29._1.Otros.Mapas;
using System;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    public class ShaperEntry
    {
        //x, y, radio_min, radio_max, dirX, dirY
        public Func<int, int, int, int, int, int, IEnumerable<Celda>> Fn { get; private set; }
        public bool tiene_direccion { get; private set; }
        public bool sin_centro { get; private set; }

        internal ShaperEntry(Func<int, int, int, int, int, int, IEnumerable<Celda>> fn, bool _tiene_direccion, bool _sin_centro)
        {
            Fn = fn;
            tiene_direccion = _tiene_direccion;
            sin_centro = _sin_centro;
        }
    }
}
