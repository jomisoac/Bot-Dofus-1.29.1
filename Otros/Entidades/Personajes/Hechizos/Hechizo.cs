/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Properties;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos
{
    public class Hechizo
    {
        public int id { get; private set; }
        public string nombre { get; private set; }
        public byte nivel { get; private set; }
        public char posicion { get; private set; }
        private Dictionary<byte, HechizoStats> statsHechizos;//nivel, informacion

        public Hechizo(int _id, string _nombre, byte _nivel, char _posicion)
        {
            id = _id;
            nombre = _nombre;
            nivel = _nivel;
            posicion = _posicion;
            statsHechizos = new Dictionary<byte, HechizoStats>();
            get_Agregar_Hechizo_Stats(nivel, get_Datos_Hechizo(id, nivel));
        }

        public HechizoStats get_Stats_Por_Nivel(byte lvl)
        {
            return statsHechizos[lvl];
        }

        public Dictionary<byte, HechizoStats> get_Hechizo_Stats()
        {
            return statsHechizos;
        }

        public void get_Agregar_Hechizo_Stats(byte nivel, HechizoStats stats)
        {
            if (statsHechizos.ContainsKey(nivel))
                statsHechizos.Remove(nivel);
            statsHechizos.Add(nivel, stats);
        }

        private HechizoStats get_Datos_Hechizo(int id_hechizo, int nivel_hechizo)
        {
            XElement xml = XElement.Parse(Resources.hechizos);
            HechizoStats hechizo_stats = new HechizoStats();
            List<string> datos_hechizo = Extensiones.get_Dividir_Matrices(xml.Elements("HECHIZO").Where(e => int.Parse(e.Element("id").Value) == id_hechizo).Elements("nivel" + nivel_hechizo).Select(e => e.Value).FirstOrDefault(), '[', ']', ',');

            hechizo_stats.coste_pa = datos_hechizo[2] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[2]);
            hechizo_stats.alcanze_minimo = datos_hechizo[3] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[3]);
            hechizo_stats.alcanze_maximo = datos_hechizo[4] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[4]);

            hechizo_stats.es_lanzado_linea = datos_hechizo[7] == "null" ? false : bool.Parse(datos_hechizo[7]);
            hechizo_stats.es_lanzado_con_vision = datos_hechizo[8] == "null" ? false : bool.Parse(datos_hechizo[8]);
            hechizo_stats.es_celda_vacia = datos_hechizo[9] == "null" ? false : bool.Parse(datos_hechizo[9]);
            hechizo_stats.es_alcanze_modificable = datos_hechizo[10] == "null" ? false : bool.Parse(datos_hechizo[10]);

            hechizo_stats.lanzamientos_por_turno = datos_hechizo[12] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[12]);
            hechizo_stats.lanzamientos_por_objetivo = datos_hechizo[13] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[13]);
            hechizo_stats.intervalo = datos_hechizo[14] == "null" ? Convert.ToByte(0) : byte.Parse(datos_hechizo[14]);
            hechizo_stats.areaAfectados = datos_hechizo[15] == "null" ? new Zonas[0] : Zonas.get_Analizar_Zonas(datos_hechizo[15]);
            return hechizo_stats;
        }
    }
}
