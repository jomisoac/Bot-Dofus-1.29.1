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
        public string mapa_data { get; set; }
        public Dictionary<int, Celda> celdas;
        public List<Personaje> personajes;

        public event Action<int> celda_actualizada;
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
                    int obj = layerObject2Interactive ? layerObject2 : -1;
                    if ((f / 10) == 297)
                        Console.WriteLine(tipo_caminable);
                    celdas.Add(f / 10, new Celda(id, f/10, tipo_caminable, IsSightBlocker, obj));
                }

            }
        }

        public void evento_Celda_Actualizada(int celda_id)
        {
            celda_actualizada?.Invoke(celda_id);
        }
    }
}
