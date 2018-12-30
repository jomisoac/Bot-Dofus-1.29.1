using System.Collections.Generic;
using Bot_Dofus_1._29._1.Otros.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos
{
    class HechizoShape
    {
        public static IEnumerable<Celda> Get_Lista_Celdas_Rango_Hechizo(int sourceCellId, HechizoStats spellLevel, Mapa mapa, int additionalRange = 0)
        {
            int range = spellLevel.alcanze_maximo + (spellLevel.es_alcanze_modificable ? additionalRange : 0);

            if (spellLevel.es_lanzado_linea)
            {
                return Shaper.Cruz(mapa.get_Celda_X_Coordenadas(sourceCellId), mapa.get_Celda_Y_Coordenadas(sourceCellId), spellLevel.alcanze_minimo, range, mapa);
            }
            else
            {
                return Shaper.Anillo(mapa.get_Celda_X_Coordenadas(sourceCellId), mapa.get_Celda_Y_Coordenadas(sourceCellId), spellLevel.alcanze_minimo, range, mapa);
            }
        }
    }
}
