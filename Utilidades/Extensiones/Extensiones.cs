using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Utilidades.Extensiones
{
    public static class Extensiones
    {
        public static readonly string[] lista_mods = { "Nemetacum", "Seydlex", "Sisuphos", "Toblik", "Falgoryn", };

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
