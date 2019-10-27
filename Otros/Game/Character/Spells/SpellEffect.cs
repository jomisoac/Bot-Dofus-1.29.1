namespace Bot_Dofus_1._29._1.Otros.Game.Character.Spells
{
    public class SpellEffect
    {
        public int id { get; set; }
        public Zones zona_efecto { get; set; }

        public SpellEffect(int _id, Zones zona)
        {
            id = _id;
            zona_efecto = zona;
        }
    }
}
