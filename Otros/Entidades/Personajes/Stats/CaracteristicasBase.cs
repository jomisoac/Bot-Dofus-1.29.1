/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/
namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Stats
{
    public class CaracteristicasBase
    {
        public int base_personaje { get; set; } = 0;
        public int equipamiento { get; set; } = 0;
        public int dones { get; set; } = 0;
        public int boost { get; set; } = 0;

        public int total_Stats => base_personaje + equipamiento + dones + boost;
        public CaracteristicasBase(int _base_personaje) => base_personaje = _base_personaje;

        public CaracteristicasBase(int _base_personaje, int _equipamiento, int _dones, int _boost)
        {
            base_personaje = _base_personaje;
            equipamiento = _equipamiento;
            dones = _dones;
            boost = _boost;
        }

        public void actualizar_Stats(int _base_personaje, int _equipamiento, int _dones, int _boost)
        {
            base_personaje = _base_personaje;
            equipamiento = _equipamiento;
            dones = _dones;
            boost = _boost;
        }
    }
}
