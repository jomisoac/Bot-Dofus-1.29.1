using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Peleadores
{
    public class Luchadores
    {
        public int id { get; set; }
        public int celda_id { get; set; }
        public byte equipo { get; set; }
        public bool esta_vivo { get; set; }
        public int vida_actual { get; set; }
        public int vida_maxima { get; set; }
        public byte pa { get; private set; }
        public byte pm { get; private set; }

        public int porcentaje_vida => (int)((double)vida_actual / vida_maxima) / 100;

        public Luchadores(int _id, byte _equipo)
        {
            id = _id;
            equipo = _equipo;
        }

        public Luchadores(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, int _celda_id, int _vida_maxima, byte _equipo)
        {
            id = _id;
            esta_vivo = _esta_vivo;
            vida_actual = _vida_actual;
            pa = _pa;
            pm = _pm;
            celda_id = _celda_id;
            vida_maxima = _vida_maxima;
            equipo = _equipo;
        }

        public void get_Actualizar_Luchador(int _id, bool _esta_vivo, int _vida_actual, byte _pa, byte _pm, int _celda_id, int _vida_maxima)
        {
            id = _id;
            esta_vivo = _esta_vivo;
            vida_actual = _vida_actual;
            pa = _pa;
            pm = _pm;
            celda_id = _celda_id;
            vida_maxima = _vida_maxima;
        }
    }
}
