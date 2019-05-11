using System;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Transporte
{
    class PaqueteAtributo : Attribute
    {
        public string paquete;

        public PaqueteAtributo(string _paquete) => paquete = _paquete;
    }
}
