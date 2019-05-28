using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using System;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes.Hechizos
{
    public class HechizoEfecto
    {
        public int id { get; set; }
        public List<object> parametros { get; set; }
        public Zonas zona_efecto { get; set; }

        public HechizoEfecto(int _id, List<object> _parametros, Zonas zona)
        {
            id = _id;
            parametros = _parametros;
            zona_efecto = zona;
        }

        public static HechizoEfecto Parse(string efecto, Zonas zona)
        {
            if (efecto == "-1")
                return null;

            string[] effectArgs = efecto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                List<object> _params = new List<object>();
                int Id = int.Parse(effectArgs[0]);

                for (int i = 1; i < effectArgs.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            if (effectArgs[i] == "-1")
                                _params.Add(null);
                            else
                                _params.Add(int.Parse(effectArgs[i]));
                        break;

                        default:
                            _params.Add(effectArgs[i]);
                        break;
                    }
                }

                return new HechizoEfecto(Id, _params, zona);
            }
            catch { return null; }
        }
    }
}
