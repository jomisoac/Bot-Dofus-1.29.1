using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot_Dofus_1._29._1.Utilities.Extensions
{
    public static class Extensions
    {
        public static string cadena_Amigable(this AccountStates accountStatus)
        {
            switch (accountStatus)
            {
                case AccountStates.CONNECTED:
                    return "Connecté";
                case AccountStates.DISCONNECTED:
                    return "Deconnecté";
                case AccountStates.EXCHANGE:
                    return "Echange";
                case AccountStates.FIGHTING:
                    return "Combat";
                case AccountStates.GATHERING:
                    return "Recolte";
                case AccountStates.MOVING:
                    return "Deplacement";
                case AccountStates.CONNECTED_INACTIVE:
                    return "Inactif";
                case AccountStates.STORAGE:
                    return "Stockage";
                case AccountStates.DIALOG:
                    return "Dialogue";
                case AccountStates.BUYING:
                    return "Achat";
                case AccountStates.SELLING:
                    return "Vente";
                case AccountStates.REGENERATION:
                    return "Regeneration";
                default:
                    return "-";
            }
        }

        public static string FullMessageException(this Exception ex)
        {
            Exception e = ex;
            StringBuilder s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + e.Message);
                s.AppendLine("Stacktrace:");
                s.AppendLine(e.StackTrace);
                s.AppendLine();
                e = e.InnerException;
            }
            return s.ToString();
        }

        public static T get_Or<T>(this Table table, string key, DataType type, T orValue)
        {
            DynValue flag = table.Get(key);

            if (flag.IsNil() || flag.Type != type)
                return orValue;

            try
            {
                return (T)flag.ToObject(typeof(T));
            }
            catch
            {
                return orValue;
            }
        }

        public static Dictionary<MapaTeleportCeldas, List<short>> Add(this Dictionary<MapaTeleportCeldas, List<short>> cells, short cellId)
        {
            short[] topCells = new short[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 36 };
            short[] rightCells = new short[] { 28, 57, 86, 115, 144, 173, 231, 202, 260, 289, 318, 347, 376, 405, 434 };
            short[] bottomCells = new short[] { 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463 };
            short[] leftCells = new short[] { 15, 44, 73, 102, 131, 160, 189, 218, 247, 276, 305, 334, 363, 392, 421, 450 };

            if (topCells.Contains(cellId))
            {
                if (cells.ContainsKey(MapaTeleportCeldas.TOP))
                    cells[MapaTeleportCeldas.TOP].Add(cellId);
                else
                {
                    cells.Add(MapaTeleportCeldas.TOP, new List<short>());
                    cells[MapaTeleportCeldas.TOP].Add(cellId);
                }
            }

            if (rightCells.Contains(cellId))
            {
                if (cells.ContainsKey(MapaTeleportCeldas.RIGHT))
                    cells[MapaTeleportCeldas.RIGHT].Add(cellId);
                else
                {
                    cells.Add(MapaTeleportCeldas.RIGHT, new List<short>());
                    cells[MapaTeleportCeldas.RIGHT].Add(cellId);
                }
            }

            if (bottomCells.Contains(cellId))
            {
                if (cells.ContainsKey(MapaTeleportCeldas.BOTTOM))
                    cells[MapaTeleportCeldas.BOTTOM].Add(cellId);
                else
                {
                    cells.Add(MapaTeleportCeldas.BOTTOM, new List<short>());
                    cells[MapaTeleportCeldas.BOTTOM].Add(cellId);
                }
            }

            if (leftCells.Contains(cellId))
            {
                if (cells.ContainsKey(MapaTeleportCeldas.LEFT))
                    cells[MapaTeleportCeldas.LEFT].Add(cellId);
                else
                {
                    cells.Add(MapaTeleportCeldas.LEFT, new List<short>());
                    cells[MapaTeleportCeldas.LEFT].Add(cellId);
                }
            }

            return cells;
        }
    }
}
