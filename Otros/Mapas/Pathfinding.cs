using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    class Pathfinding
    {
        public static int get_Celda_Y_Coordenadas(int celda_id)
        {
            int loc5 = celda_id / ((15 * 2) - 1);
            int loc6 = celda_id - (loc5 * ((15 * 2) - 1));
            int loc7 = loc6 % 15;
            return loc5 - loc7;
        }

        public static int get_Celda_X_Coordenadas(int celda_id) => ((celda_id - (15 - 1) * get_Celda_Y_Coordenadas(celda_id)) / 15);
    }
}
