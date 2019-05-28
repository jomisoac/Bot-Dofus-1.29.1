using System;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Stats
{
    public class CaracteristicasInformacion
    {
        public double experiencia_actual { get; set; }
        public double experiencia_minima_nivel { get; set; }
        public double experiencia_siguiente_nivel { get; set; }
        public int kamas { get; set; }
        public int energia_actual { get; set; }
        public int maxima_energia { get; set; }
        public int vitalidad_actual { get; set; }
        public int vitalidad_maxima { get; set; }
        public CaracteristicasBase iniciativa { get; set; }
        public CaracteristicasBase prospeccion { get; set; }
        public CaracteristicasBase puntos_accion { get; set; }
        public CaracteristicasBase puntos_movimiento { get; set; }
        public CaracteristicasBase vitalidad { get; set; }
        public CaracteristicasBase sabiduria { get; set; }
        public CaracteristicasBase fuerza { get; set; }
        public CaracteristicasBase inteligencia { get; set; }
        public CaracteristicasBase suerte { get; set; }
        public CaracteristicasBase agilidad { get; set; }
        public CaracteristicasBase alcanze { get; set; }
        public CaracteristicasBase criaturas_invocables { get; set; }
        public int porcentaje_vida => vitalidad_maxima == 0 ? 0 : (int)((double)vitalidad_actual / vitalidad_maxima * 100);
    }
}
