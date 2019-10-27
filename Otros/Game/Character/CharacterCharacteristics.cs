/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Character
{
    public class CharacterCharacteristics : IEliminable
    {
        public double experiencia_actual { get; set; }
        public double experiencia_minima_nivel { get; set; }
        public double experiencia_siguiente_nivel { get; set; }
        public int energia_actual { get; set; }
        public int maxima_energia { get; set; }
        public int vitalidad_actual { get; set; }
        public int vitalidad_maxima { get; set; }
        public CharacterStats iniciativa { get; set; }
        public CharacterStats prospeccion { get; set; }
        public CharacterStats puntos_accion { get; set; }
        public CharacterStats puntos_movimiento { get; set; }
        public CharacterStats vitalidad { get; set; }
        public CharacterStats sabiduria { get; set; }
        public CharacterStats fuerza { get; set; }
        public CharacterStats inteligencia { get; set; }
        public CharacterStats suerte { get; set; }
        public CharacterStats agilidad { get; set; }
        public CharacterStats alcanze { get; set; }
        public CharacterStats criaturas_invocables { get; set; }
        public int porcentaje_vida => vitalidad_maxima == 0 ? 0 : (int)((double)vitalidad_actual / vitalidad_maxima * 100);

        public CharacterCharacteristics()
        {
            iniciativa = new CharacterStats(0, 0, 0, 0);
            prospeccion = new CharacterStats(0, 0, 0, 0);
            puntos_accion = new CharacterStats(0, 0, 0, 0);
            puntos_movimiento = new CharacterStats(0, 0, 0, 0);
            vitalidad = new CharacterStats(0, 0, 0, 0);
            sabiduria = new CharacterStats(0, 0, 0, 0);
            fuerza = new CharacterStats(0, 0, 0, 0);
            inteligencia = new CharacterStats(0, 0, 0, 0);
            suerte = new CharacterStats(0, 0, 0, 0);
            agilidad = new CharacterStats(0, 0, 0, 0);
            alcanze = new CharacterStats(0, 0, 0, 0);
            criaturas_invocables = new CharacterStats(0, 0, 0, 0);
        }

        public void Clear()
        {
            experiencia_actual = 0;
            experiencia_minima_nivel = 0;
            experiencia_siguiente_nivel = 0;
            energia_actual = 0;
            maxima_energia = 0;
            vitalidad_actual = 0;
            vitalidad_maxima = 0;

            iniciativa.Clear();
            prospeccion.Clear();
            puntos_accion.Clear();
            puntos_movimiento.Clear();
            vitalidad.Clear();
            sabiduria.Clear();
            fuerza.Clear();
            inteligencia.Clear();
            suerte.Clear();
            agilidad.Clear();
            alcanze.Clear();
            criaturas_invocables.Clear();
        }
    }
}
