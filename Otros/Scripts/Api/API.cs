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
    public class API : IDisposable
    {
        public InventarioApi inventario { get; private set; }
        public PersonajeApi personaje { get; private set; }
        public MapaApi mapa { get; private set; }
        public NpcAPI npc { get; private set; }
        public PeleaApi pelea { get; private set; }
        private bool disposed;

        public API(Account cuenta, ManejadorAcciones manejar_acciones)
        {
            inventario = new InventarioApi(cuenta, manejar_acciones);
            personaje = new PersonajeApi(cuenta);
            mapa = new MapaApi(cuenta, manejar_acciones);
            npc = new NpcAPI(cuenta, manejar_acciones);
            pelea = new PeleaApi(cuenta, manejar_acciones);
        }

        #region Zona Dispose
        ~API() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    inventario.Dispose();
                    personaje.Dispose();
                    mapa.Dispose();
                    npc.Dispose();
                    pelea.Dispose();
                }

                inventario = null;
                personaje = null;
                mapa = null;
                npc = null;
                pelea = null;
                disposed = true;
            }
        }
        #endregion
    }
}
