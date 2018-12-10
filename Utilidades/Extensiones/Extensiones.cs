using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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
                Campo = f, Atributo = a
            }).Where(a => ((DescriptionAttribute)a.Atributo).Description == descripcion).SingleOrDefault();
            return campo == null ? default(T) : (T)campo.Campo.GetRawConstantValue();
        }
    }
}
