using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos
{
    public class HechizoStats
    {
        public byte coste_pa { get; set; }
        public byte alcanze_minimo { get; set; }
        public byte alcanze_maximo { get; set; }

        public bool es_lanzado_linea { get; set; }
        public bool es_lanzado_con_vision { get; set; }
        public bool es_celda_vacia { get; set; }
        public bool es_alcanze_modificable { get; set; }

        public byte lanzamientos_por_turno { get; set; }
        public byte lanzamientos_por_objetivo { get; set; }
        public byte intervalo { get; set; }
    }
}
