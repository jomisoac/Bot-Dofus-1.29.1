using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Stats;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Monstruos
{
    class Monstruo : Entidad
    {
        public int id { get; set; } = 0;
        public int template { get; set; } = 0;
        public CaracteristicasInformacion caracteristicas { get; set; }

        public void Dispose()
        {
            caracteristicas = null;
        }
    }
}