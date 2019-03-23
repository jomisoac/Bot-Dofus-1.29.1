using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Utilidades.Extensiones
{
    public static class Extensiones
    {
        public static List<string> get_Dividir_Matrices(string string_analizado, char comienzo_matriz, char final_matriz, char separador)
        {
            List<string> resultado = new List<string>();
            resultado.Add(string.Empty);
            int corchete = 0;

            foreach (char v in string_analizado)
            {
                if (v != ' ')
                {
                    if (v == comienzo_matriz)
                    {
                        corchete++;
                        if (corchete < 2)
                            continue;
                    }
                    else if (v == final_matriz)
                    {
                        corchete--;
                        if (corchete == 0)
                            continue;
                    }

                    if (v == separador && corchete == 1)
                        resultado.Add(string.Empty);
                    else
                        resultado[resultado.Count - 1] += v;
                }
            }
            return resultado;
        }
    }
}
