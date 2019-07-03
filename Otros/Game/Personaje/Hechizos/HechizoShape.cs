using Bot_Dofus_1._29._1.Otros.Mapas;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    public class HechizoShape
    {

        public static IEnumerable<Celda> Get_Lista_Celdas_Rango_Hechizo(Celda celda, HechizoStats spellLevel, Mapa mapa, int rango_adicional = 0)
        {
            int rango_maximo = spellLevel.alcanze_maximo + (spellLevel.es_alcanze_modificable ? rango_adicional : 0);
            
            if (spellLevel.es_lanzado_linea)
                return Shaper.Cruz(celda.x, celda.y, spellLevel.alcanze_minimo, rango_maximo, mapa);
            else
                return Shaper.Anillo(celda.x, celda.y, spellLevel.alcanze_minimo, rango_maximo, mapa);
        }
    }
}
