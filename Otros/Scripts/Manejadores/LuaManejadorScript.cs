using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Manejadores
{
    public class LuaManejadorScript : IDisposable
    {
        public Script script { get; private set; }

        public void cargar_Desde_Archivo(string ruta_archivo, Action antes_de_archivo)
        {
            script = new Script();
            antes_de_archivo();
            script.DoFile(ruta_archivo);
        }

        public IEnumerable<Table> get_Entradas_Funciones(string nombre_funcion)
        {
            DynValue funcion = script.Globals.Get(nombre_funcion);

            if (funcion.IsNil() || funcion.Type != DataType.Function)
                return null;

            DynValue resultado = script.Call(funcion);

            if (resultado.Type != DataType.Table)
                return null;
            return resultado.Table.Values.Where(f => f.Type == DataType.Table).Select(f => f.Table);
        }

        public T get_Global_Or<T>(string key, DataType tipo, T valor_or)
        {
            DynValue global = script.Globals.Get(key);

            if (global.IsNil() || global.Type != tipo)
                return valor_or;
            try
            {
                return (T)global.ToObject(typeof(T));
            }
            catch
            {
                return valor_or;
            }
        }
        
        public DynValue get_Global_como_Dyn_Valor(string key) => script.Globals.Get(key);
        public T get_Global_Or<T>(string key, T or) => es_Global(key) ? (T)script.Globals[key] : or;
        public T get_Global<T>(string key) => es_Global(key) ? (T)script.Globals[key] : default(T);
        public bool es_Global(string key) => script.Globals[key] != null;
        public void Set_Global(string key, object value) => script.Globals[key] = value;

        ~LuaManejadorScript() => Dispose(false);
        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                script = null;
            }
        }
    }
}
