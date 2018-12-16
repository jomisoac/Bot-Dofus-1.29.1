using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Personajes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Mapa
    {
        public int id { get; set; } = 0;
        public int fecha { get; set; } = 0;
        public int anchura { get; set; } = 15;
        public int altura { get; set; } = 17;
        public string mapa_data { get; set; }
        public Dictionary<int, Celda> celdas;
        public Dictionary<int, Personaje> personajes;
        
        private readonly XElement archivo_mapa;

        public Mapa(string paquete)
        {
            string[] _loc3 = paquete.Split('|');

            id = int.Parse(_loc3[0]);
            fecha = int.Parse(_loc3[1]);


            archivo_mapa = XElement.Load("mapas/" + id + "_0" + fecha + ".xml");
            mapa_data = archivo_mapa.Element("MAPA_DATA").Value;
            archivo_mapa = null;
            descompilar_Celdas_Mapa();
        }

        public void descompilar_Celdas_Mapa()
        {
            if (celdas != null)
                celdas.Clear();
            else
                celdas = new Dictionary<int, Celda>();

            for (int f = 0; f < mapa_data.Length; f += 10)
            {
                string celda_data = mapa_data.Substring(f, 10);
                List<byte> celdas_informacion = new List<byte>();

                for (int i = 0; i < celda_data.Length; ++i)
                    celdas_informacion.Add(Convert.ToByte(Compressor.index_Hash(char.Parse(celda_data[i].ToString()))));

                bool esta_activa = (celdas_informacion[0] & 32) >> 5 != 0;
                if (esta_activa)
                {
                    byte tipo_caminable = Convert.ToByte((celdas_informacion[2] & 56) >> 3);
                    bool IsSightBlocker = (celdas_informacion[0] & 1) != 0;

                    int layerObject2 = ((celdas_informacion[0] & 2) << 12) + ((celdas_informacion[7] & 1) << 12) + (celdas_informacion[8] << 6) + celdas_informacion[9];
                    bool layerObject2Interactive = ((celdas_informacion[7] & 2) >> 1) != 0;
                    int id_celda = f/10;
                    celdas.Add(id_celda, new Celda(id_celda, tipo_caminable, IsSightBlocker, layerObject2Interactive ? layerObject2 : -1, layerObject2Interactive));
                }
            }
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
