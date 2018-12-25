using System;
using Bot_Dofus_1._29._1.Otros.Entidades.Stats;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Monstruos
{
    public class Monstruo : Entidad
    {
        public int id { get; set; } = 0;
        public int template_id { get; set; } = 0;
        public CaracteristicasInformacion caracteristicas { get; set; }
        public int celda_id { get; set; }
        bool disposed;

        public Monstruo(int _id, int _template, int _celda_id)
        {
            id = _id;
            template_id = _template;
            celda_id = _celda_id;
        }

        ~Monstruo() => Dispose(false);

        public bool es_agresivo() => template_id == 791 && template_id == 253;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    caracteristicas.Dispose();
                }
                caracteristicas = null;
                disposed = true;
            }
        }
    }
}