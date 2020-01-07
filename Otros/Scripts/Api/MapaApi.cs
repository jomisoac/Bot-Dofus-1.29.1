using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Mapas;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using MoonSharp.Interpreter;
using System;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Api
{
    [MoonSharpUserData]
    public class MapaApi : IDisposable
    {
        private Account cuenta;
        private ActionsManager manejador_acciones;
        private bool disposed = false;

        public MapaApi(Account _cuenta, ActionsManager _manejador_acciones)
        {
            cuenta = _cuenta;
            manejador_acciones = _manejador_acciones;
        }

        public bool cambiarMapa(string posicion)
        {
            if (cuenta.Is_Busy())
                return false;

            if (!ChangeMapAction.TryParse(posicion, out ChangeMapAction accion))
            {
                cuenta.Logger.LogError("MapApi", $"Vérifier le changement de carte {posicion}");
                return false;
            }

            manejador_acciones.enqueue_Accion(accion, true);
            return true;
        }

        public bool moverCelda(short celda_id)
        {
            if (celda_id < 0 || celda_id > cuenta.game.map.mapCells.Length)
                return false;

            if (!cuenta.game.map.GetCellFromId(celda_id).IsWalkable() || cuenta.game.map.GetCellFromId(celda_id).isInLineOfSight)
                return false;

            manejador_acciones.enqueue_Accion(new MoverCeldaAccion(celda_id), true);
            return true;
        }

        public bool enCelda(short celda_id) => cuenta.game.character.celda.cellId == celda_id;
        public bool enMapa(string coordenadas) => cuenta.game.map.esta_En_Mapa(coordenadas);
        public string actualMapa() => cuenta.game.map.mapId.ToString();
        public string actualPosicion() => cuenta.game.map.GetCoordinates;

        #region Zona Dispose
        ~MapaApi() => Dispose(false);
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
