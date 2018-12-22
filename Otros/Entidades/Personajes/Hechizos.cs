/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/
namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes
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
