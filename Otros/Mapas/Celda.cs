namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Celda
    {
        public int id_celda { get; set; } = 0;
        public int objeto_interactivo_id { get; set; } = 0;
        public byte tipo_caminable { get; set; } = 0;
        public bool es_linea_vision { get; set; } = false;
        public bool object2Movement { get; set; } = false;

        public Celda(int _id_celda)
        {
            id_celda = _id_celda;
        }
    }
}
