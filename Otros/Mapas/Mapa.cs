using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Npcs;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;
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
    public class Mapa : IDisposable
    {
        public int id { get; set; }
        public byte anchura { get; set; }
        public byte altura { get; set; }
        public string data { get; set; }
        public Celda[] celdas;
        private Cuenta cuenta { get; set; }
        public bool verificar_Mapa_Actual(int mapa_id) => mapa_id == id;
        public Dictionary<int, Personaje> personajes;
        public Dictionary<int, Monstruo> monstruos;
        public Dictionary<int, Npcs> npcs;
        private bool disposed = false;

        public event Action mapa_actualizado;

        public Mapa(Cuenta _cuenta) => cuenta = _cuenta;
        
        public void get_Actualizar_Mapa(string paquete)
        {
            get_Personajes().Clear();
            get_Monstruos().Clear();

            string[] _loc3 = paquete.Split('|');
            id = int.Parse(_loc3[0]);

            FileInfo mapa_archivo = new FileInfo("mapas/" + id + ".xml");
            if (mapa_archivo.Exists)
            {
                XElement archivo_mapa = XElement.Load(mapa_archivo.FullName);
                anchura = byte.Parse(archivo_mapa.Element("ANCHURA").Value);
                altura = byte.Parse(archivo_mapa.Element("ALTURA").Value);
                data = archivo_mapa.Element("MAPA_DATA").Value;

                archivo_mapa = null;//limpia la memoria
                descomprimir_mapa();
            }
            else
            {
                cuenta.conexion.get_Desconectar_Socket();
                cuenta.logger.log_Error("Mapa", $"Archivo de mapa no encontrado bot desconectado, id mapa: {id}");
            }
            mapa_archivo = null;

            mapa_actualizado?.Invoke();
        }

        public Celda get_Celda_Id(int celda_id) => celdas[celda_id];
        
        public bool get_Puede_Luchar_Contra_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios) => get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios).Count > 0;

        public List<Monstruo> get_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios)
        {
            List<Monstruo> grupos_monstruos_disponibles = new List<Monstruo>();

            foreach (Monstruo grupo_monstruos in get_Monstruos().Values)
            {
                if (grupo_monstruos.get_Total_Monstruos < monstruos_minimos || grupo_monstruos.get_Total_Monstruos > monstruos_maximos)
                    continue;

                if (grupo_monstruos.get_Total_Nivel_Grupo < nivel_minimo || grupo_monstruos.get_Total_Nivel_Grupo > nivel_maximo)
                    continue;

                bool es_valido = true;

                if (monstruos_prohibidos != null)
                {
                    for (int i = 0; i < monstruos_prohibidos.Count; i++)
                    {
                        if (grupo_monstruos.get_Contiene_Monstruo(monstruos_prohibidos[i]))
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
                        if (!grupo_monstruos.get_Contiene_Monstruo(monstruos_obligatorios[i]))
                        {
                            es_valido = false;
                            break;
                        }
                    }
                }

                if (es_valido)
                    grupos_monstruos_disponibles.Add(grupo_monstruos);
            }
            return grupos_monstruos_disponibles;
        }

        public Celda get_Coordenadas(int x, int y) => celdas.FirstOrDefault(celda => celda.x == x && celda.y == y);

        #region Metodos de descompresion
        public void descomprimir_mapa()
        {
            celdas = new Celda[data.Length / 10];
            string valores_celda;

            for (int i = 0; i < data.Length; i += 10)
            {
                valores_celda = data.Substring(i, 10);
                celdas[i / 10] = descompimir_Celda(valores_celda, Convert.ToInt16(i / 10));
            }
        }

        public Celda descompimir_Celda(string celda_data, short id_celda)
        {
            byte[] informacion_celda = new byte[celda_data.Length];
            for (int i = 0; i < celda_data.Length; i++)
                informacion_celda[i] = Convert.ToByte(Hash.get_Hash(celda_data[i]));

            TipoCelda tipo = (TipoCelda)((informacion_celda[2] & 56) >> 3);
            bool es_linea_vision = (informacion_celda[0] & 1) == 1;
            bool tiene_objeto_interactivo = ((informacion_celda[7] & 2) >> 1) != 0;
            short layer_objeto_2_num = Convert.ToInt16(((informacion_celda[0] & 2) << 12) + ((informacion_celda[7] & 1) << 12) + (informacion_celda[8] << 6) + informacion_celda[9]);
            short layer_objeto_1_num = Convert.ToInt16(((informacion_celda[0] & 4) << 11) + ((informacion_celda[4] & 1) << 12) + (informacion_celda[5] << 6) + informacion_celda[6]);
            byte nivel = Convert.ToByte(informacion_celda[1] & 15);
            byte slope = Convert.ToByte((informacion_celda[4] & 60) >> 2);

            return new Celda(id_celda, tipo, es_linea_vision, nivel, slope, tiene_objeto_interactivo ? layer_objeto_2_num : Convert.ToInt16(-1), layer_objeto_1_num, layer_objeto_2_num, this);
        }
        #endregion

        #region metodos monstruos
        public void agregar_Monstruo(Monstruo monstruo)
        {
            if (monstruos == null)
                monstruos = new Dictionary<int, Monstruo>();

            if (!monstruos.ContainsKey(monstruo.id))
                monstruos.Add(monstruo.id, monstruo);
        }

        public void eliminar_Monstruo(int id)
        {
            if (monstruos != null)
            {
                if (monstruos.ContainsKey(id))
                    monstruos.Remove(id);

                if (monstruos.Count <= 0)//si no tiene ninguno volvera a ser nulo
                    monstruos = null;
            }
        }

        public Dictionary<int, Monstruo> get_Monstruos()
        {
            if (monstruos == null)
                return new Dictionary<int, Monstruo>();
            return monstruos;
        }
        #endregion

        #region metodos npcs
        public void agregar_Npc(Npcs npc)
        {
            if (npcs == null)
                npcs = new Dictionary<int, Npcs>();
            if (!npcs.ContainsKey(npc.id))
                npcs.Add(npc.id, npc);
        }

        public void eliminar_Npc(int id)
        {
            if (npcs != null)
            {
                if (npcs.ContainsKey(id))
                    npcs.Remove(id);
                if (npcs.Count <= 0)//si no tiene ninguno volvera a ser nulo
                    npcs = null;
            }
        }

        public Dictionary<int, Npcs> get_Npcs()
        {
            if (npcs == null)
                return new Dictionary<int, Npcs>();
            return npcs;
        }
        #endregion

        #region metodos personajes
        public void agregar_Personaje(Personaje personaje)
        {
            if (personajes == null)
                personajes = new Dictionary<int, Personaje>();
            if (!personajes.ContainsKey(personaje.id))
                personajes.Add(personaje.id, personaje);
        }

        public void eliminar_Personaje(int id)
        {
            if (personajes != null)
            {
                if (personajes.ContainsKey(id))
                    personajes.Remove(id);
                if (personajes.Count <= 0)//si no tiene ninguno volvera a ser nulo
                    personajes = null;
            }
        }

        public Dictionary<int, Personaje> get_Personajes()
        {
            if (personajes == null)
                return new Dictionary<int, Personaje>();
            return personajes;
        }
        #endregion

        #region Zona Dispose
        ~Mapa() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (personajes != null)
                    personajes.Clear();
                if (monstruos != null)
                    monstruos.Clear();
                celdas = null;
                personajes = null;
                cuenta = null;
                data = null;
                disposed = true;
            }
        }
        #endregion
    }
}
