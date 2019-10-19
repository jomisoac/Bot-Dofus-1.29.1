using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.Linq;

namespace Bot_Dofus_1._29._1.Utilidades.Extensiones
{
    public static class Extensiones
    {
        public static string cadena_Amigable(this EstadoCuenta estado)
        {
            switch (estado)
            {
                case EstadoCuenta.CONECTANDO:
                    return "Connecté";
                case EstadoCuenta.DESCONECTADO:
                    return "Deconnecté";
                case EstadoCuenta.INTERCAMBIO:
                    return "Echange";
                case EstadoCuenta.LUCHANDO:
                    return "Combat";
                case EstadoCuenta.RECOLECTANDO:
                    return "Recolte";
                case EstadoCuenta.MOVIMIENTO:
                    return "Deplacement";
                case EstadoCuenta.CONECTADO_INACTIVO:
                    return "Inactif";
                case EstadoCuenta.ALMACENAMIENTO:
                    return "Stockage";
                case EstadoCuenta.DIALOGANDO:
                    return "Dialog";
                case EstadoCuenta.COMPRANDO:
                    return "Achat";
                case EstadoCuenta.VENDIENDO:
                    return "Vente";
                case EstadoCuenta.REGENERANDO:
                    return "Regeneration";
                default:
                    return "-";
            }
        }

        public static T get_Or<T>(this Table table, string key, DataType type, T orValue)
        {
            DynValue bandera = table.Get(key);

            if (bandera.IsNil() || bandera.Type != type)
                return orValue;

            try
            {
                return (T)bandera.ToObject(typeof(T));
            }
            catch
            {
                return orValue;
            }
        }

        public static Dictionary<MapaTeleportCeldas, List<short>> Add(this Dictionary<MapaTeleportCeldas, List<short>> cells, short cellId)
        {
            short[] topCells = new short[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,36 };
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
