using System;

namespace Bot_Dofus_1._29._1.Controles.ControlMapa
{
    [Serializable]
    [Flags]
    public enum CeldaEstado
    {
        NINGUNO = 0,
        CAMINABLE = 1,
        NO_CAMINABLE = 2,
        PELEA_EQUIPO_AZUL = 4,
        PELEA_EQUIPO_ROJO = 8,
        CELDA_TELEPORT = 16,
        OBJETO_INTERACTIVO = 32
    }
}
