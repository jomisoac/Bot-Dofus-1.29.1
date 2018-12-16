using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Personajes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Mapa
    {
        public int id { get; set; } = 0;
        public int fecha { get; set; } = 0;
        public int anchura { get; set; } = 15;
        public int altura { get; set; } = 17;
        public string mapa_data { get; set; }
        public Celda[] celdas;
        public Dictionary<int, Personaje> personajes;
        
        private readonly XElement archivo_mapa;

        public Mapa(string paquete)
        {
            string[] _loc3 = paquete.Split('|');

            id = int.Parse(_loc3[0]);
            fecha = int.Parse(_loc3[1]);
            
            archivo_mapa = XElement.Load("mapas/" + id + "_0" + fecha + ".xml");
            anchura = int.Parse(archivo_mapa.Element("ANCHURA").Value);
            altura = int.Parse(archivo_mapa.Element("ALTURA").Value);
            mapa_data = archivo_mapa.Element("MAPA_DATA").Value;

            archivo_mapa = null;//limpia la memoria
            descompilar_mapa();
        }

        public void descompilar_mapa()
        {
            celdas = new Celda[mapa_data.Length/10];
            string celda_data;

            for (int i = 0; i < mapa_data.Length; i += 10)
            {
                celda_data = mapa_data.Substring(i, 10);
                celdas[i/10] = descompimir_Celda(celda_data, i/10);
            }
        }

        public Celda descompimir_Celda(string celda_data, int id_celda)
        {
            Celda celda = new Celda(id_celda);

            byte[] informacion_celda = new byte[celda_data.Length];
            for (int i = 0; i < celda_data.Length; i++)
            {
                informacion_celda[i] = Convert.ToByte(Compressor.index_Hash(celda_data[i]));
            }

            celda.tipo_caminable = Convert.ToByte((informacion_celda[2] & 56) >> 3);
            celda.es_linea_vision = (informacion_celda[0] & 1) != 0;
            celda.objeto_interactivo_id = ((informacion_celda[7] & 2) >> 1) != 0 ? ((informacion_celda[0] & 2) << 12) + ((informacion_celda[7] & 1) << 12) + (informacion_celda[8] << 6) + informacion_celda[9] : -1;
            celda.object2Movement = ((informacion_celda[7] & 2) >> 1) != 0;
            return celda;
        }

        public void agregar_Personaje(Personaje personaje)
        {
            if (personajes == null)
                personajes = new Dictionary<int, Personaje>();
            if (!personajes.ContainsKey(personaje.id))
                personajes.Add(personaje.id, personaje);
        }

        public void eliminar_Personaje(int id_personaje)
        {
            if (personajes != null)
            {
                if (personajes.ContainsKey(id_personaje))
                    personajes.Remove(id_personaje);
                if (personajes.Count >= 0)//si no tiene ninguno volvera a ser nulo
                    personajes = null;
            }
        }

        public Dictionary<int, Personaje> get_Personajes()
        {
            if (personajes == null)
                return new Dictionary<int, Personaje>();
            return personajes;
        }
    }
}
