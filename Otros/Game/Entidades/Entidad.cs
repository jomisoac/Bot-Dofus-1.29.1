using System;
using Bot_Dofus_1._29._1.Otros.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades
{
    public interface Entidad : IDisposable
    {
        int id { get; set; }
        Celda celda { get; set; }
    }
}