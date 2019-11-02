using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using Bot_Dofus_1._29._1.Utilities.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public class Map : IEliminable, IDisposable
    {
        public int mapId { get; set; }
        public byte mapWidth { get; set; }
        public byte mapHeight { get; set; }
        public sbyte x { get; set; }
        public sbyte y { get; set; }
        public Cell[] mapCells;
        public Dictionary<MapaTeleportCeldas, List<short>> CellsTeleport;

        /** Concurrent para forzar thread-safety **/
        public ConcurrentDictionary<int, Entidad> entities;
        public ConcurrentDictionary<int, ObjetoInteractivo> interactives;

        public event Action mapRefreshEvent;
        public event Action entitiesRefreshEvent;
        private bool _disposed = false;

        public Map()
        {
            entities = new ConcurrentDictionary<int, Entidad>();
            interactives = new ConcurrentDictionary<int, ObjetoInteractivo>();
            CellsTeleport = new Dictionary<MapaTeleportCeldas, List<short>>();
        }

        public void GetRefreshMap(string prmPacket)
        {
            entities.Clear();
            interactives.Clear();
            CellsTeleport.Clear();

            string[] _loc3 = prmPacket.Split('|');
            mapId = int.Parse(_loc3[0]);

            FileInfo mapFile = new FileInfo("mapas/" + mapId + ".xml");
            if (mapFile.Exists)
            {
                XElement xmlMap = XElement.Load(mapFile.FullName);
                mapWidth = byte.Parse(xmlMap.Element("ANCHURA").Value);
                mapHeight = byte.Parse(xmlMap.Element("ALTURA").Value);
                x = sbyte.Parse(xmlMap.Element("X").Value);
                y = sbyte.Parse(xmlMap.Element("Y").Value);

                Task.Run(() => DecompressMap(xmlMap.Element("MAPA_DATA").Value)).Wait();
                Task.Run(() => getTeleportCell(mapCells)).Wait();
            }
        }

        public string GetCoordinates => $"[{x},{y}]";
        public Cell GetCellFromId(short prmCellId) => mapCells[prmCellId];
        public bool esta_En_Mapa(string _coordenadas) => _coordenadas == mapId.ToString() || _coordenadas == GetCoordinates;
        public Cell GetCellByCoordinates(int prmX, int prmY) => mapCells.FirstOrDefault(cell => cell.x == prmX && cell.y == prmY);
        public bool get_Puede_Luchar_Contra_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios) => get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios).Count > 0;

        // si el destino es una celda teleport, aunque haya un monstruo encima de la celda no causara agresion
        public List<Cell> celdas_ocupadas() => entities.Values.Where(c => c is Monstruos).Select(c => c.celda).ToList();
        public List<Npcs> lista_npcs() => entities.Values.Where(n => n is Npcs).Select(n => n as Npcs).ToList();
        public List<Monstruos> lista_monstruos() => entities.Values.Where(n => n is Monstruos).Select(n => n as Monstruos).ToList();
        public List<Personajes> lista_personajes() => entities.Values.Where(n => n is Personajes).Select(n => n as Personajes).ToList();

        public List<Monstruos> get_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios)
        {
            List<Monstruos> grupos_monstruos_disponibles = new List<Monstruos>();

            foreach (Monstruos grupo_monstruo in lista_monstruos())
            {
                if (grupo_monstruo.get_Total_Monstruos < monstruos_minimos || grupo_monstruo.get_Total_Monstruos > monstruos_maximos)
                    continue;

                if (grupo_monstruo.get_Total_Nivel_Grupo < nivel_minimo || grupo_monstruo.get_Total_Nivel_Grupo > nivel_maximo)
                    continue;

                if (grupo_monstruo.celda.cellType == CellTypes.TELEPORT_CELL)
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

        public void GetMapRefreshEvent() => mapRefreshEvent?.Invoke();
        public void GetEntitiesRefreshEvent() => entitiesRefreshEvent?.Invoke();

        #region Metodos de descompresion
        public void DecompressMap(string prmMapData)
        {
            mapCells = new Cell[prmMapData.Length / 10];
            string cells_value;

            for (int i = 0; i < prmMapData.Length; i += 10)
            {
                cells_value = prmMapData.Substring(i, 10);
                mapCells[i / 10] = DecompressCell(cells_value, Convert.ToInt16(i / 10));
            }
        }

        public Cell DecompressCell(string prmCellData, short prmCellId)
        {
            byte[] cellInformations = new byte[prmCellData.Length];

            for (int i = 0; i < prmCellData.Length; i++)
                cellInformations[i] = Convert.ToByte(Hash.get_Hash(prmCellData[i]));

            CellTypes tipo = (CellTypes)((cellInformations[2] & 56) >> 3);
            bool activa = (cellInformations[0] & 32) >> 5 != 0;
            bool es_linea_vision = (cellInformations[0] & 1) != 1;
            bool tiene_objeto_interactivo = ((cellInformations[7] & 2) >> 1) != 0;
            short layer_objeto_2_num = Convert.ToInt16(((cellInformations[0] & 2) << 12) + ((cellInformations[7] & 1) << 12) + (cellInformations[8] << 6) + cellInformations[9]);
            short layer_objeto_1_num = Convert.ToInt16(((cellInformations[0] & 4) << 11) + ((cellInformations[4] & 1) << 12) + (cellInformations[5] << 6) + cellInformations[6]);
            byte nivel = Convert.ToByte(cellInformations[1] & 15);
            byte slope = Convert.ToByte((cellInformations[4] & 60) >> 2);


            return new Cell(prmCellId, activa, tipo, es_linea_vision, nivel, slope, tiene_objeto_interactivo ? layer_objeto_2_num : Convert.ToInt16(-1), layer_objeto_1_num, layer_objeto_2_num, this);
        }


        public void getTeleportCell(Cell[] cells)
        {
            List<Cell> cellsToManipulate = cells.ToList().Where(c => c.IsTeleportCell()).ToList();
            cellsToManipulate.ForEach(cell =>
            {
                CellsTeleport.Add(cell.cellId);
            });
        }

        public bool haveTeleport()
        {
            return CellsTeleport.Count > 0;
        }

        public string TransformToCellId(string[] cellsDirection)
        {
            StringBuilder st = new StringBuilder();
            for (int i = 0; i < cellsDirection.Length; i++)
            {
                switch (cellsDirection[i])
                {
                    case "RIGHT":
                        st.Append(CellsTeleport[MapaTeleportCeldas.RIGHT].First());
                        break;
                    case "LEFT":
                        st.Append(CellsTeleport[MapaTeleportCeldas.LEFT].First());
                        break;
                    case "TOP":
                        st.Append(CellsTeleport[MapaTeleportCeldas.TOP].First());
                        break;
                    case "BOTTOM":
                        st.Append(CellsTeleport[MapaTeleportCeldas.BOTTOM].First());
                        break;
                    default:
                        break;
                }
                if (i < cellsDirection.Length - 1)
                    st.Append('|');
            }
            return st.ToString();
        }
        public string TransformToCellId(string cellDirection)
        {
            StringBuilder st = new StringBuilder();
            switch (cellDirection)
            {
                case "RIGHT":
                    st.Append(CellsTeleport[MapaTeleportCeldas.RIGHT].First());
                    break;
                case "LEFT":
                    st.Append(CellsTeleport[MapaTeleportCeldas.LEFT].First());
                    break;
                case "TOP":
                    st.Append(CellsTeleport[MapaTeleportCeldas.TOP].First());
                    break;
                case "BOTTOM":
                    st.Append(CellsTeleport[MapaTeleportCeldas.BOTTOM].First());
                    break;
                default:
                    break;
            }
            return st.ToString();
        }

        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Map() => Dispose(false);

        public void Clear()
        {
            mapId = 0;
            x = 0;
            y = 0;
            entities.Clear();
            interactives.Clear();
            CellsTeleport.Clear();
            mapCells = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            entities.Clear();
            interactives.Clear();

            mapCells = null;
            entities = null;
            CellsTeleport = null;
            _disposed = true;
        }
        #endregion
    }
}
