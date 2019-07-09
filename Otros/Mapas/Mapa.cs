using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Mapa : IEliminable, IDisposable
    {
        public int id { get; set; }
        public byte anchura { get; set; }
        public byte altura { get; set; }
        public sbyte x { get; set; }
        public sbyte y { get; set; }
        public Celda[] celdas;

        /** Concurrent para forzar thread-safety **/
        public ConcurrentDictionary<int, Entidad> entidades;
        public ConcurrentDictionary<int, ObjetoInteractivo> interactivos;

        public event Action mapa_actualizado;
        public event Action entidades_actualizadas;
        private bool disposed = false;

        public Mapa()
        {
            entidades = new ConcurrentDictionary<int, Entidad>();
            interactivos = new ConcurrentDictionary<int, ObjetoInteractivo>();
        }

        public void get_Actualizar_Mapa(string paquete)
        {
            entidades.Clear();
            interactivos.Clear();

            string[] _loc3 = paquete.Split('|');
            id = int.Parse(_loc3[0]);

            FileInfo mapa_archivo = new FileInfo("mapas/" + id + ".xml");
            if (mapa_archivo.Exists)
            {
                XElement archivo_mapa = XElement.Load(mapa_archivo.FullName);
                anchura = byte.Parse(archivo_mapa.Element("ANCHURA").Value);
                altura = byte.Parse(archivo_mapa.Element("ALTURA").Value);
                x = sbyte.Parse(archivo_mapa.Element("X").Value);
                y = sbyte.Parse(archivo_mapa.Element("Y").Value);

                Task.Run(() => descomprimir_mapa(archivo_mapa.Element("MAPA_DATA").Value)).Wait();
            }
        }

        public string coordenadas => $"[{x},{y}]";
        public Celda get_Celda_Id(short celda_id) => celdas[celda_id];
        public bool esta_En_Mapa(string _coordenadas) => _coordenadas == id.ToString() || _coordenadas == coordenadas;
        public Celda get_Celda_Por_Coordenadas(int x, int y) => celdas.FirstOrDefault(celda => celda.x == x && celda.y == y);
        public bool get_Puede_Luchar_Contra_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios) => get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios).Count > 0;

        // si el destino es una celda teleport, aunque haya un monstruo encima de la celda no causara agresion
        public List<Celda> celdas_ocupadas() => entidades.Values.Select(c => c.celda).ToList();
        public List<Npcs> lista_npcs() => entidades.Values.Where(n => n is Npcs).Select(n => n as Npcs).ToList();
        public List<Monstruos> lista_monstruos() => entidades.Values.Where(n => n is Monstruos).Select(n => n as Monstruos).ToList();
        public List<Personajes> lista_personajes() => entidades.Values.Where(n => n is Personajes).Select(n => n as Personajes).ToList();

        public List<Monstruos> get_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios)
        {
            List<Monstruos> grupos_monstruos_disponibles = new List<Monstruos>();

            foreach (Monstruos grupo_monstruo in lista_monstruos())
            {
                if (grupo_monstruo.get_Total_Monstruos < monstruos_minimos || grupo_monstruo.get_Total_Monstruos > monstruos_maximos)
                    continue;

                if (grupo_monstruo.get_Total_Nivel_Grupo < nivel_minimo || grupo_monstruo.get_Total_Nivel_Grupo > nivel_maximo)
                    continue;

                if (grupo_monstruo.celda.tipo == TipoCelda.CELDA_TELEPORT)
                    continue;

                bool es_valido = true;

                if (monstruos_prohibidos != null)
                {
                    for (int i = 0; i < monstruos_prohibidos.Count; i++)
                    {
                        if (grupo_monstruo.get_Contiene_Monstruo(monstruos_prohibidos[i]))
                        {
                            es_valido = false;
                            break;
                        }
                    }
                }

                if (monstruos_obligatorios != null && es_valido)
                {
                    for (int i = 0; i < monstruos_obligatorios.Count; i++)
                    {
                        if (!grupo_monstruo.get_Contiene_Monstruo(monstruos_obligatorios[i]))
                        {
                            es_valido = false;
                            break;
                        }
                    }
                }

                if (es_valido)
                    grupos_monstruos_disponibles.Add(grupo_monstruo);
            }
            return grupos_monstruos_disponibles;
        }

        public void get_Evento_Mapa_Cambiado() => mapa_actualizado?.Invoke();
        public void evento_Entidad_Actualizada() => entidades_actualizadas?.Invoke();

        #region Metodos de descompresion
        public void descomprimir_mapa(string mapa_data)
        {
            celdas = new Celda[mapa_data.Length / 10];
            string valores_celda;

            for (int i = 0; i < mapa_data.Length; i += 10)
            {
                valores_celda = mapa_data.Substring(i, 10);
                celdas[i / 10] = descompimir_Celda(valores_celda, Convert.ToInt16(i / 10));
            }
        }

        public Celda descompimir_Celda(string celda_data, short id_celda)
        {
            byte[] informacion_celda = new byte[celda_data.Length];

            for (int i = 0; i < celda_data.Length; i++)
                informacion_celda[i] = Convert.ToByte(Hash.get_Hash(celda_data[i]));

            TipoCelda tipo = (TipoCelda)((informacion_celda[2] & 56) >> 3);
            bool activa = (informacion_celda[0] & 32) >> 5 != 0;
            bool es_linea_vision = (informacion_celda[0] & 1) != 1;
            bool tiene_objeto_interactivo = ((informacion_celda[7] & 2) >> 1) != 0;
            short layer_objeto_2_num = Convert.ToInt16(((informacion_celda[0] & 2) << 12) + ((informacion_celda[7] & 1) << 12) + (informacion_celda[8] << 6) + informacion_celda[9]);
            short layer_objeto_1_num = Convert.ToInt16(((informacion_celda[0] & 4) << 11) + ((informacion_celda[4] & 1) << 12) + (informacion_celda[5] << 6) + informacion_celda[6]);
            byte nivel = Convert.ToByte(informacion_celda[1] & 15);
            byte slope = Convert.ToByte((informacion_celda[4] & 60) >> 2);

            return new Celda(id_celda, activa, tipo, es_linea_vision, nivel, slope, tiene_objeto_interactivo ? layer_objeto_2_num : Convert.ToInt16(-1), layer_objeto_1_num, layer_objeto_2_num, this);
        }
        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Mapa() => Dispose(false);

        public void limpiar()
        {
            id = 0;
            x = 0;
            y = 0;
            entidades.Clear();
            interactivos.Clear();
            celdas = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            entidades.Clear();
            interactivos.Clear();
            
            celdas = null;
            entidades = null;
            disposed = true;
        }
        #endregion
    }
}
