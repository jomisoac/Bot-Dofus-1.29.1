using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory.Enums;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Inventario;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;
using System;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class InventarioApi : IDisposable
    {
        private Account cuenta;
        private ActionsManager manejar_acciones;
        private bool disposed = false;

        public InventarioApi(Account _cuenta, ActionsManager _manejar_acciones)
        {
            cuenta = _cuenta;
            manejar_acciones = _manejar_acciones;
        }

        public int pods() => cuenta.game.character.inventario.pods_actuales;
        public int podsMaximos() => cuenta.game.character.inventario.pods_maximos;
        public int podsPorcentaje() => cuenta.game.character.inventario.porcentaje_pods;
        public bool tieneObjeto(int modelo_id) => cuenta.game.character.inventario.get_Objeto_Modelo_Id(modelo_id) != null;

        public bool utilizar(int modelo_id)
        {
            InventoryObject objeto = cuenta.game.character.inventario.get_Objeto_Modelo_Id(modelo_id);

            if (objeto == null)
                return false;

            manejar_acciones.enqueue_Accion(new UtilizarObjetoAccion(modelo_id), true);
            return true;
        }

        public bool equipar(int modelo_id)
        {
            InventoryObject objeto = cuenta.game.character.inventario.get_Objeto_Modelo_Id(modelo_id);

            if (objeto == null || objeto.posicion != InventorySlots.NOT_EQUIPPED)
                return false;

            manejar_acciones.enqueue_Accion(new EquiparItemAccion(modelo_id), true);
            return true;
        }

        #region Zona Dispose
        ~InventarioApi() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                cuenta = null;
                manejar_acciones = null;
                disposed = true;
            }
        }
        #endregion
    }
}
