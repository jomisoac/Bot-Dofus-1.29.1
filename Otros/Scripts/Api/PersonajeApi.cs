using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class PersonajeApi
    {
        private Cuenta cuenta;

        public PersonajeApi(Cuenta _cuenta) => cuenta = _cuenta;

        public string nombre() => cuenta.juego.personaje.nombre;
        public byte nivel() => cuenta.juego.personaje.nivel;
        public int experiencia() => cuenta.juego.personaje.porcentaje_experiencia;
        public int kamas() => cuenta.juego.personaje.kamas;
    }
}
