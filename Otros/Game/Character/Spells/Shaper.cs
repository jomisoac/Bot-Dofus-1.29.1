using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Game.Character.Spells
{
    class Shaper
    {
        public static IEnumerable<Cell> Circulo(int x, int y, int radio_minimo, int radio_maximo, Map mapa)
        {
            List<Cell> rango = new List<Cell>();

            if (radio_minimo == 0)
                rango.Add(mapa.GetCellByCoordinates(x, y));

            for (int radio = radio_minimo == 0 ? 1 : radio_minimo; radio <= radio_maximo; radio++)
            {
                for (int i = 0; i < radio; i++)
                {
                    int r = radio - i;
                    rango.Add(mapa.GetCellByCoordinates(x + i, y - r));
                    rango.Add(mapa.GetCellByCoordinates(x + r, y + i));
                    rango.Add(mapa.GetCellByCoordinates(x - i, y + r));
                    rango.Add(mapa.GetCellByCoordinates(x - r, y - i));
                }
            }

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Cell> Linea(int x, int y, int radio_minimo, int radio_maximo, Map mapa)
        {
            List<Cell> rango = new List<Cell>();

            for (int i = radio_minimo; i <= radio_maximo; i++)
                rango.Add(mapa.GetCellByCoordinates(x * i, y * i));

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Cell> Cruz(int x, int y, int radio_minimo, int radio_maximo, Map mapa)
        {
            List<Cell> rango = new List<Cell>();

            if (radio_minimo == 0)
                rango.Add(mapa.GetCellByCoordinates(x, y));

            for (int i = (radio_minimo == 0 ? 1 : radio_minimo); i <= radio_maximo; i++)
            {
                rango.Add(mapa.GetCellByCoordinates(x - i, y));
                rango.Add(mapa.GetCellByCoordinates(x + i, y));
                rango.Add(mapa.GetCellByCoordinates(x, y - i));
                rango.Add(mapa.GetCellByCoordinates(x, y + i));
            }

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Cell> Anillo(int x, int y, int radio_minimo, int radio_maximo, Map mapa)
        {
            List<Cell> rango = new List<Cell>();

            if (radio_minimo == 0)
                rango.Add(mapa.GetCellByCoordinates(x, y));

            for (int radius = radio_minimo == 0 ? 1 : radio_minimo; radius <= radio_maximo; radius++)
            {
                for (int i = 0; i < radius; i++)
                {
                    int r = radius - i;
                    rango.Add(mapa.GetCellByCoordinates(x + i, y - r));
                    rango.Add(mapa.GetCellByCoordinates(x + r, y + i));
                    rango.Add(mapa.GetCellByCoordinates(x - i, y + r));
                    rango.Add(mapa.GetCellByCoordinates(x - r, y - i));
                }
            }
            return rango.Where(c => c != null);
        }
    }
}
