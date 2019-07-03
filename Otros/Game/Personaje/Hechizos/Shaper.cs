using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    class Shaper
    {
        public static IEnumerable<Celda> Circulo(int x, int y, int radio_minimo, int radio_maximo, Mapa mapa)
        {
            List<Celda> rango = new List<Celda>();

            if (radio_minimo == 0)
                rango.Add(mapa.get_Celda_Por_Coordenadas(x, y));

            for (int radio = radio_minimo == 0 ? 1 : radio_minimo; radio <= radio_maximo; radio++)
            {
                for (int i = 0; i < radio; i++)
                {
                    int r = radio - i;
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x + i, y - r));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x + r, y + i));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x - i, y + r));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x - r, y - i));
                }
            }

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Celda> Linea(int x, int y, int radio_minimo, int radio_maximo, Mapa mapa)
        {
            List<Celda> rango = new List<Celda>();

            for (int i = radio_minimo; i <= radio_maximo; i++)
                rango.Add(mapa.get_Celda_Por_Coordenadas(x * i, y * i));

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Celda> Cruz(int x, int y, int radio_minimo, int radio_maximo, Mapa mapa)
        {
            List<Celda> rango = new List<Celda>();

            if (radio_minimo == 0)
                rango.Add(mapa.get_Celda_Por_Coordenadas(x, y));

            for (int i = (radio_minimo == 0 ? 1 : radio_minimo); i <= radio_maximo; i++)
            {
                rango.Add(mapa.get_Celda_Por_Coordenadas(x - i, y));
                rango.Add(mapa.get_Celda_Por_Coordenadas(x + i, y));
                rango.Add(mapa.get_Celda_Por_Coordenadas(x, y - i));
                rango.Add(mapa.get_Celda_Por_Coordenadas(x, y + i));
            }

            return rango.Where(c => c != null);
        }

        public static IEnumerable<Celda> Anillo(int x, int y, int radio_minimo, int radio_maximo, Mapa mapa)
        {
            List<Celda> rango = new List<Celda>();

            if (radio_minimo == 0)
                rango.Add(mapa.get_Celda_Por_Coordenadas(x, y));

            for (int radius = radio_minimo == 0 ? 1 : radio_minimo; radius <= radio_maximo; radius++)
            {
                for (int i = 0; i < radius; i++)
                {
                    int r = radius - i;
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x + i, y - r));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x + r, y + i));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x - i, y + r));
                    rango.Add(mapa.get_Celda_Por_Coordenadas(x - r, y - i));
                }
            }
            return rango.Where(c => c != null);
        }
    }
}
