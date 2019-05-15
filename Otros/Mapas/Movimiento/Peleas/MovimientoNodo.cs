namespace Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas
{
    public class MovimientoNodo
    {
        public short celda_inicial { get; private set; }
        public bool alcanzable { get; private set; }
        public PeleaCamino camino { get; set; }

        public MovimientoNodo(short _celda_inicial, bool _alcanzable)
        {
            celda_inicial = _celda_inicial;
            alcanzable = _alcanzable;
        }
    }
}
