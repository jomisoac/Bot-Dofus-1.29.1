/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje
{
    public class PersonajeStats : IEliminable
    {
        public int base_personaje { get; set; } = 0;
        public int equipamiento { get; set; } = 0;
        public int dones { get; set; } = 0;
        public int boost { get; set; } = 0;

        public int total_stats => base_personaje + equipamiento + dones + boost;
        public PersonajeStats(int _base_personaje) => base_personaje = _base_personaje;
        public PersonajeStats(int _base_personaje, int _equipamiento, int _dones, int _boost) => actualizar_Stats(_base_personaje, _equipamiento, _dones, _boost);

        public void actualizar_Stats(int _base_personaje, int _equipamiento, int _dones, int _boost)
        {
            base_personaje = _base_personaje;
            equipamiento = _equipamiento;
            dones = _dones;
            boost = _boost;
        }

        public void limpiar()
        {
            base_personaje = 0;
            equipamiento = 0;
            dones = 0;
            boost = 0;
        }
    }
}
