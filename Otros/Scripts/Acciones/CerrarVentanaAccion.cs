using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    class CerrarVentanaAccion : AccionesScript
    {
        internal override Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            if (cuenta.esta_dialogando())
            {
                cuenta.conexion.enviar_Paquete("EV");
                return resultado_procesado;
            }

            return resultado_hecho;
        }
    }
}
