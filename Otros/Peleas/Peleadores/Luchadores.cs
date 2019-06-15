using Bot_Dofus_1._29._1.Otros.Mapas;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Peleadores
{
    public class Luchadores
    {
        public int id { get; set; }
        public Celda celda { get; set; }
        public byte equipo { get; set; }
        public bool esta_vivo { get; set; }
        public int vida_actual { get; set; }
        public int vida_maxima { get; set; }
        public byte pa { get; set; }
        public byte pm { get; set; }
        public int id_invocador { get; set; }

        public int porcentaje_vida => (int)((double)vida_actual / vida_maxima) / 100;

        public Luchadores(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, Celda _celda, int _vida_maxima, byte _equipo) => get_Actualizar_Luchador(_id, _esta_vivo, _vida_actual, _pa, _pm, _celda, _vida_maxima, _equipo);

        //invocaciones
        public Luchadores(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, Celda _celda, int _vida_maxima, byte _equipo, int _id_invocador) => get_Actualizar_Luchador(_id, _esta_vivo, _vida_actual, _pa, _pm, _celda, _vida_maxima, _equipo, _id_invocador);

        public void get_Actualizar_Luchador(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, Celda _celda_id, int _vida_maxima, byte _equipo)
        {
            id = _id;
            esta_vivo = _esta_vivo;
            vida_actual = _vida_actual;
            pa = _pa;
            pm = _pm;
            celda = _celda_id;
            vida_maxima = _vida_maxima;
            equipo = _equipo;
        }

        public void get_Actualizar_Luchador(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, Celda _celda, int _vida_maxima, byte _equipo, int _id_invocador)
        {
            get_Actualizar_Luchador(_id, _esta_vivo, _vida_actual, _pa, _pm, _celda, _vida_maxima, _equipo);
            id_invocador = _id_invocador;
        }
    }
}
