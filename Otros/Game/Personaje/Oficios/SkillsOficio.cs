using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Oficios
{
    public class SkillsOficio
    {
        public short id { get; private set; }
        public byte cantidad_minima { get; private set; }
        public byte cantidad_maxima { get; private set; }
        public ObjetoInteractivoModelo interactivo_modelo { get; private set; }
        public bool es_craft { get; private set; } = true;
        public float tiempo { get; private set; }

        public SkillsOficio(short _id, byte _cantidad_minima, byte _cantidad_maxima, float _tiempo)
        {
            id = _id;
            cantidad_minima = _cantidad_minima;
            cantidad_maxima = _cantidad_maxima;
           
            ObjetoInteractivoModelo _modelo = ObjetoInteractivoModelo.get_Modelo_Por_Habilidad(id);
            if (_modelo != null)
            {
                interactivo_modelo = _modelo;

                if(interactivo_modelo.recolectable)
                    es_craft = false;
            }
            tiempo = _tiempo;
        }

        public void set_Actualizar(short _id, byte _cantidad_minima, byte _cantidad_maxima, float _tiempo)
        {
            id = _id;
            cantidad_minima = _cantidad_minima;
            cantidad_maxima = _cantidad_maxima;
            tiempo = _tiempo;
        }
    }
}