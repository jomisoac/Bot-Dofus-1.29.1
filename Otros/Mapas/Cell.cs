using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using System;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Cell
    {
        public short cellId { get; private set; } = 0;
        public bool isActive { get; private set; } = false;
        public CellTypes cellType { get; private set; } = CellTypes.NOT_WALKABLE;
        public bool isInLineOfSight { get; private set; } = false;
        public byte layer_ground_nivel { get; private set; }
        public byte layer_ground_slope { get; private set; }
        public short layer_object_1_num { get; private set; }
        public short layer_object_2_num { get; private set; }
        public ObjetoInteractivo interactiveObject { get; private set; }
        public int x { get; private set; } = 0;
        public int y { get; private set; } = 0;

        /** pathfinder **/
        public int coste_h { get; set; } = 0;
        public int coste_g { get; set; } = 0;
        public int coste_f { get; set; } = 0;
        public Cell parentNode { get; set; } = null;

        public static readonly int[] TeleportTexturesSpritesId = { 1030, 1029, 1764, 2298, 745 };

        public Cell(short prmCellId, bool prmIsActive, CellTypes prmCellType, bool prmIsInLineOfSight, byte _nivel, byte _slope, short prmInteractiveObjectId, short _layer_object_1_num, short _layer_object_2_num, Map prmMap)
        {
            cellId = prmCellId;
            isActive = prmIsActive;
            cellType = prmCellType;

            layer_object_1_num = _layer_object_1_num;
            layer_object_2_num = _layer_object_2_num;

            isInLineOfSight = prmIsInLineOfSight;
            layer_ground_nivel = _nivel;
            layer_ground_slope = _slope;

            if (prmInteractiveObjectId != -1)
            {
                interactiveObject = new ObjetoInteractivo(prmInteractiveObjectId, this);
                prmMap.interactives.TryAdd(cellId, interactiveObject);
            }

            byte mapWidth = prmMap.mapWidth;
            int loc5 = cellId / ((mapWidth * 2) - 1);
            int loc6 = cellId - (loc5 * ((mapWidth * 2) - 1));
            int loc7 = loc6 % mapWidth;
            y = loc5 - loc7;
            x = (cellId - ((mapWidth - 1) * y)) / mapWidth;
        }

        public int GetDistanceBetweenCells(Cell prmDestinationCell) => Math.Abs(x - prmDestinationCell.x) + Math.Abs(y - prmDestinationCell.y);
        public bool AreCellsOnLine(Cell prmDestinationCell) => x == prmDestinationCell.x || y == prmDestinationCell.y;

        public char GetCharDirection(Cell prmCell)
        {
            if (x == prmCell.x)
                return prmCell.y < y ? (char)(3 + 'a') : (char)(7 + 'a');
            else if (y == prmCell.y)
                return prmCell.x < x ? (char)(1 + 'a') : (char)(5 + 'a');
            
            else if (x > prmCell.x)
                return y > prmCell.y ? (char)(2 + 'a') : (char)(0 + 'a');
            else if (x < prmCell.x)
                return y < prmCell.y ? (char)(6 + 'a') : (char)(4 + 'a');

            throw new Exception("Error direct non trouvée");
        }

        public bool IsTeleportCell() => TeleportTexturesSpritesId.Contains(layer_object_1_num) || TeleportTexturesSpritesId.Contains(layer_object_2_num);
        public bool IsInteractiveCell() => cellType == CellTypes.INTERACTIVE_OBJECT || interactiveObject != null;
        public bool IsWalkable() => isActive && cellType != CellTypes.NOT_WALKABLE && !IsInteractiveWalkable();
        public bool IsInteractiveWalkable() => cellType == CellTypes.INTERACTIVE_OBJECT || interactiveObject != null && !interactiveObject.modelo.caminable;
    }
}
