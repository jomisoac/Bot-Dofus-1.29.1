using System;

namespace Bot_Dofus_1._29._1.Controles.ControlMapa
{
    [Serializable]
    [Flags]
    public enum Modo_Dibujo
    {
        NINGUNO = 0,
        MOVIMIENTOS = 1,
        PELEAS = 2,
        CELDAS_TELEPORT = 4,
        OTROS = 8,
        TODO = 15,
    }
}
