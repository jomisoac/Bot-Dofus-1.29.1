using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Banderas
{
    public class FuncionPersonalizada : Bandera
    {
        public DynValue funcion { get; private set; }

        public FuncionPersonalizada(DynValue _funcion) => funcion = _funcion;
    }
}
