namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    public class HechizoEfecto
    {
        public int id { get; set; }
        public Zonas zona_efecto { get; set; }

        public HechizoEfecto(int _id, Zonas zona)
        {
            id = _id;
            zona_efecto = zona;
        }
    }
}
