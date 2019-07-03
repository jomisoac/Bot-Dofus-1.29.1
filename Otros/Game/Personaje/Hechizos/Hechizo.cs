using System.Collections.Generic;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    public class Hechizo
    {
        public short id { get; private set; }
        public string nombre { get; private set; }
        public byte nivel { get; set; }
        public Dictionary<byte, HechizoStats> statsHechizos;//nivel, informacion

        public static Dictionary<short, Hechizo> hechizos_cargados = new Dictionary<short, Hechizo>();

        public Hechizo(short _id, string _nombre)
        {
            id = _id;
            nombre = _nombre;
            statsHechizos = new Dictionary<byte, HechizoStats>();

            hechizos_cargados.Add(id, this);
        }

        public void get_Agregar_Hechizo_Stats(byte _nivel, HechizoStats stats)
        {
            if (statsHechizos.ContainsKey(_nivel))
                statsHechizos.Remove(_nivel);

            statsHechizos.Add(_nivel, stats);
        }

        public HechizoStats get_Stats() => statsHechizos[nivel];
        public static Hechizo get_Hechizo(short id) => hechizos_cargados[id];
    }
}
