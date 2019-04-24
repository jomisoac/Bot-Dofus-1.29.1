using System;

namespace Bot_Dofus_1._29._1.Comun.Frames.Transporte
{
    class PaqueteAtributo : Attribute
    {
        public string paquete;

        public PaqueteAtributo(string _paquete) => paquete = _paquete;
    }
}
