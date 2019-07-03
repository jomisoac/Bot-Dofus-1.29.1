using System;

namespace Bot_Dofus_1._29._1.Otros.Mapas.Entidades
{
    public interface Entidad : IDisposable
    {
        int id { get; set; }
        Celda celda { get; set; }
    }
}