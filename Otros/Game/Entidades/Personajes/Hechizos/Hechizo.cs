using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
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
            string datos_hechizo = xml.Elements("HECHIZO").Where(e => int.Parse(e.Element("id").Value) == id_hechizo).Elements("nivel" + nivel_hechizo).Select(e => e.Value).FirstOrDefault();
            List<string> hechizo_separado = Extensiones.get_Dividir_Matrices(datos_hechizo, '[', ']', ',');

            int contador = 0;
            bool es_critico = false;
            List<string> efectos_normales = new List<string>() { string.Empty };
            List<string> efectos_criticos = new List<string>() { string.Empty };

            try
            {
                for (int i = 0; i < datos_hechizo.Length; i++)
                {
                    if (datos_hechizo[i] == '[')
                    {
                        contador++;
                        continue;
                    }
                    if (datos_hechizo[i] == ']')
                    {
                        contador--;
                        continue;
                    }

                    if (datos_hechizo[i] == ',' && efectos_normales[efectos_normales.Count - 1] != "" && contador == 2 && !es_critico)
                        efectos_normales.Add(string.Empty);
                    else if (datos_hechizo[i] == ',' && efectos_criticos[efectos_criticos.Count - 1] != "" && contador == 2)
                        efectos_criticos.Add(string.Empty);

                    if (efectos_normales[efectos_normales.Count - 1] != "" && contador < 2)
                        es_critico = true;
                    else if (efectos_criticos[efectos_criticos.Count - 1] != "" & contador < 2)
                        break;

                    if (contador == 3 && !es_critico)
                        efectos_normales[efectos_normales.Count - 1] += datos_hechizo[i];
                    else if (contador == 3 && es_critico)
                        efectos_criticos[efectos_criticos.Count - 1] += datos_hechizo[i];
                }
            }
            catch { }

            hechizo_stats.coste_pa = hechizo_separado[2] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[2]);
            hechizo_stats.alcanze_minimo = hechizo_separado[3] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[3]);
            hechizo_stats.alcanze_maximo = hechizo_separado[4] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[4]);

            hechizo_stats.es_lanzado_linea = hechizo_separado[7] == "-1" ? false : bool.Parse(hechizo_separado[7]);
            hechizo_stats.es_lanzado_con_vision = hechizo_separado[8] == "-1" ? false : bool.Parse(hechizo_separado[8]);
            hechizo_stats.es_celda_vacia = hechizo_separado[9] == "-1" ? false : bool.Parse(hechizo_separado[9]);
            hechizo_stats.es_alcanze_modificable = hechizo_separado[10] == "-1" ? false : bool.Parse(hechizo_separado[10]);

            hechizo_stats.lanzamientos_por_turno = hechizo_separado[12] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[12]);
            hechizo_stats.lanzamientos_por_objetivo = hechizo_separado[13] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[13]);
            hechizo_stats.intervalo = hechizo_separado[14] == "-1" ? Convert.ToByte(0) : byte.Parse(hechizo_separado[14]);
            hechizo_stats.areaAfectados = hechizo_separado[15] == "-1" ? new Zonas[0] : Zonas.get_Analizar_Zonas(hechizo_separado[15]);
            
            for (int n = 0; n < efectos_normales.Count; n++)
            {
                if (efectos_normales[n] != string.Empty)
                    hechizo_stats.efectos_normales.Add(HechizoEfecto.Parse(efectos_normales[n], hechizo_stats.areaAfectados[n]));
            }

            for (int c = 0; c < efectos_criticos.Count; c++)
            {
                if (efectos_criticos[c] != string.Empty)
                    hechizo_stats.efectos_criticos.Add(HechizoEfecto.Parse(efectos_criticos[c], hechizo_stats.areaAfectados[c]));
            }

            return hechizo_stats;
        }
    }
}
