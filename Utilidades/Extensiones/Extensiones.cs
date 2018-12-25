using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Utilidades.Extensiones
{
    public static class Extensiones
    {
        public static T get_Enum_By_Descripcion<T>(string descripcion)
        {
            Type tipo = typeof(T);
            if (!tipo.IsEnum)
                throw new ArgumentException();
            FieldInfo[] campos = tipo.GetFields();
            var campo = campos.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new
            {
                Campo = f,
                Atributo = a
            }).Where(a => ((DescriptionAttribute)a.Atributo).Description == descripcion).SingleOrDefault();
            return campo == null ? default(T) : (T)campo.Campo.GetRawConstantValue();
        }
        
        public static List<string> get_Dividir_Matrices(string string_analizado, char comienzo_matriz, char final_matriz, char separador)
        {
            List<string> resultado = new List<string>();
            resultado.Add(string.Empty);
            int proof = 0;

            foreach (char v in string_analizado)
            {
                if (v != ' ')
                {
                    if (v == comienzo_matriz)
                    {
                        proof++;
                        if (proof < 2) continue;
                    }
                    else if (v == final_matriz)
                    {
                        proof--;
                        if (proof == 0) continue;
                    }

                    if (v == separador && proof == 1)
                    {
                        resultado.Add(string.Empty);
                    }
                    else
                    {
                        resultado[resultado.Count - 1] += v;
                    }
                }
            }
            return resultado;
        }


    }
}
