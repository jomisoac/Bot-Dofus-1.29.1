using System;
using System.Collections.Generic;
using Bot_Dofus_1._29._1.Otros.Personajes;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public int id_mapa;
        public int id_celda;
        public int objeto_interactivo_id;
        public byte tipo_caminable;
        public bool es_linea_vision;
        public List<Personaje> personajes;

        public Celda(int _id_mapa, int _id_celda, byte _tipo_caminable, bool _es_linea_vision, int _objeto_interactivo_id)
        {
            id_mapa = _id_mapa;
            id_celda = _id_celda;
            tipo_caminable = _tipo_caminable;
            es_linea_vision = _es_linea_vision;
            objeto_interactivo_id = _objeto_interactivo_id;
        }

        public void agregar_Personaje(Personaje personaje)
        {
            if (personajes == null)
                personajes = new List<Personaje>();
            if (!personajes.Contains(personaje))
                personajes.Add(personaje);
        }

        public void eliminar_Personaje(Personaje personaje)
        {
            if (personajes != null)
            {
                if (personajes.Contains(personaje))
                    personajes.Remove(personaje);
                if (personajes.Count >= 0)//si no tiene ninguno volvera a ser nulo
                    personajes = null;
            }
        }

        public List<Personaje> get_Personajes()
        {
            if (personajes == null)
                return new List<Personaje>();
            return personajes;
        }
    }
}
