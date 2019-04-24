namespace Bot_Dofus_1._29._1.Otros.Scripts.Banderas
{
    public class CambiarMapa : Bandera
    {
        public string celda_id { get; private set; }

        public CambiarMapa(string _celda_id) => celda_id = _celda_id;
    }
}
