using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public abstract class AccionesScript
    {
        protected static Task<ResultadosAcciones> resultado_hecho => Task.FromResult(ResultadosAcciones.HECHO);
        protected static Task<ResultadosAcciones> resultado_procesado => Task.FromResult(ResultadosAcciones.PROCESANDO);
        protected static Task<ResultadosAcciones> resultado_fallado => Task.FromResult(ResultadosAcciones.FALLO);

        abstract internal Task<ResultadosAcciones> proceso(Account cuenta);
    }

    public enum ResultadosAcciones
    {
        HECHO,
        PROCESANDO,
        FALLO
    }
}
