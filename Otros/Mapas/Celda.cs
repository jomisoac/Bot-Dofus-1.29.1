using System;
using System.Collections.Generic;
using Bot_Dofus_1._29._1.Otros.Personajes;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public int id_celda { get; set; } = 0;
        public int objeto_interactivo_id { get; set; } = 0;
        public byte tipo_caminable { get; set; } = 0;
        public bool es_linea_vision;
        public bool object2Movement;
        public Dictionary<long, Personaje> personajes;

        public Celda(int _id_celda, byte _tipo_caminable, bool _es_linea_vision, int _objeto_interactivo_id, bool object2Movement)
        {
            id_celda = _id_celda;
            tipo_caminable = _tipo_caminable;
            es_linea_vision = _es_linea_vision;
            objeto_interactivo_id = _objeto_interactivo_id;
        }
    }
}
