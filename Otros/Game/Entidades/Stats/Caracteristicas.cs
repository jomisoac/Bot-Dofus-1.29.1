/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Stats
{
    public class Caracteristicas : IEliminable
    {
        public double experiencia_actual { get; set; }
        public double experiencia_minima_nivel { get; set; }
        public double experiencia_siguiente_nivel { get; set; }
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

        public Caracteristicas()
        {
            iniciativa = new CaracteristicasBase(0, 0, 0, 0);
            prospeccion = new CaracteristicasBase(0, 0, 0, 0);
            puntos_accion = new CaracteristicasBase(0, 0, 0, 0);
            puntos_movimiento = new CaracteristicasBase(0, 0, 0, 0);
            vitalidad = new CaracteristicasBase(0, 0, 0, 0);
            sabiduria = new CaracteristicasBase(0, 0, 0, 0);
            fuerza = new CaracteristicasBase(0, 0, 0, 0);
            inteligencia = new CaracteristicasBase(0, 0, 0, 0);
            suerte = new CaracteristicasBase(0, 0, 0, 0);
            agilidad = new CaracteristicasBase(0, 0, 0, 0);
            alcanze = new CaracteristicasBase(0, 0, 0, 0);
            criaturas_invocables = new CaracteristicasBase(0, 0, 0, 0);
        }

        public void limpiar()
        {
            experiencia_actual = 0;
            experiencia_minima_nivel = 0;
            experiencia_siguiente_nivel = 0;
            energia_actual = 0;
            maxima_energia = 0;
            vitalidad_actual = 0;
            vitalidad_maxima = 0;

            iniciativa.limpiar();
            prospeccion.limpiar();
            puntos_accion.limpiar();
            puntos_movimiento.limpiar();
            vitalidad.limpiar();
            sabiduria.limpiar();
            fuerza.limpiar();
            inteligencia.limpiar();
            suerte.limpiar();
            agilidad.limpiar();
            alcanze.limpiar();
            criaturas_invocables.limpiar();
        }
    }
}
