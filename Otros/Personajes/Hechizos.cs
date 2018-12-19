using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Personajes
{
    public class Hechizos
    {
        public int id { get; private set; }
        public string nombre { get; private set; }
        public byte nivel { get; private set; }
        public char posicion { get; private set; }

        public Hechizos(int _id, string _nombre, byte _nivel, char _posicion)
        {
            id = _id;
            nombre = _nombre;
            nivel = _nivel;
            posicion = _posicion;
        }
    }
}
