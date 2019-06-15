namespace Bot_Dofus_1._29._1.Otros.Peleas.Peleadores
{
    public class LuchadorPersonaje : Luchadores
    {
        public string nombre { get; private set; }
        public byte nivel { get; private set; }

        public LuchadorPersonaje(string _nombre, byte _nivel, Luchadores luchador) : base(luchador.id, luchador.esta_vivo, luchador.vida_actual, luchador.pa, luchador.pm, luchador.celda, luchador.vida_maxima, luchador.equipo)
        {
            nombre = _nombre;
            nivel = _nivel;
        }
    }
}