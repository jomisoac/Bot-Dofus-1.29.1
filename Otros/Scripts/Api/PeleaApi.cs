using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class PeleaApi : IDisposable
    {
        private Cuenta cuenta;
        private ManejadorAcciones manejador_acciones;
        private bool disposed = false;

        public PeleaApi(Cuenta _cuenta, ManejadorAcciones _manejador_acciones)
        {
            cuenta = _cuenta;
            manejador_acciones = _manejador_acciones;
        }

        public bool puedePelear(int monstruos_minimos = 1, int monstruos_maximos = 8, int nivel_minimo = 1, int nivel_maximo = 1000, List<int> monstruos_prohibidos = null, List<int> monstruos_obligatorios = null) => cuenta.juego.mapa.get_Puede_Luchar_Contra_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);

        public bool pelear(int monstruos_minimos = 1, int monstruos_maximos = 8, int nivel_minimo = 1, int nivel_maximo = 1000, List<int> monstruos_prohibidos = null, List<int> monstruos_obligatorios = null)
        {
            if (puedePelear(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios))
            {
                manejador_acciones.enqueue_Accion(new PeleasAccion(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios), true);
                return true;
            }

            return false;
        }

        #region Zona Dispose
        ~PeleaApi() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                cuenta = null;
                manejador_acciones = null;
                disposed = true;
            }
        }
        #endregion
    }
}
