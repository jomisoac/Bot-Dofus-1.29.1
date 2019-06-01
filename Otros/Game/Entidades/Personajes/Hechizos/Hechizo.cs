using Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Properties;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos
{
    public class Hechizo
    {
        public short id { get; private set; }
        public string nombre { get; private set; }
        public byte nivel { get; private set; }
        private Dictionary<byte, HechizoStats> statsHechizos;//nivel, informacion

        public static Dictionary<short, Hechizo> hechizos_cargados = new Dictionary<short, Hechizo>();

        public Hechizo(short _id, string _nombre)
        {
            id = _id;
            nombre = _nombre;
            statsHechizos = new Dictionary<byte, HechizoStats>();

            hechizos_cargados.Add(id, this);
        }

        public HechizoStats get_Stats(byte nivel) => statsHechizos[nivel];

        public void get_Agregar_Hechizo_Stats(byte nivel, HechizoStats stats)
        {
            if (statsHechizos.ContainsKey(nivel))
                statsHechizos.Remove(nivel);

            statsHechizos.Add(nivel, stats);
        }

        public static Hechizo get_Hechizo(short id, byte nivel)
        {
            Hechizo hechizo = hechizos_cargados[id];
            hechizo.nivel = nivel;

            return hechizo;
        }
    }
}
