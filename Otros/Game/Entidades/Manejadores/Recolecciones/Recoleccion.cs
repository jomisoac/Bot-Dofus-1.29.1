using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones
{
    public class Recoleccion : IDisposable
    {
        private Cuenta cuenta;
        private Mapa mapa;
        private ObjetoInteractivo interactivo_recolectando;
        private List<int> interactivos_no_utilizables;
        private bool robado;
        private Pathfinder pathfinder;
        private bool disposed;

        public event Action recoleccion_iniciada;
        public event Action<RecoleccionResultado> recoleccion_acabada;

        public Recoleccion(Cuenta _cuenta, Movimiento movimientos, Mapa _mapa)
        {
            cuenta = _cuenta;
            interactivos_no_utilizables = new List<int>();
            pathfinder = new Pathfinder();
            mapa = _mapa;

            movimientos.movimiento_finalizado += get_Movimiento_Finalizado;
            mapa.mapa_actualizado += evento_Mapa_Actualizado;
        }

        public bool get_Puede_Recolectar(List<short> elementos_recolectables) => get_Interactivos_Utilizables(elementos_recolectables).Count > 0;
        public void get_Cancelar_Interactivo() => interactivo_recolectando = null;

        public bool get_Recolectar(List<short> elementos)
        {
            if (cuenta.esta_ocupado || interactivo_recolectando != null)
                return false;

            foreach (KeyValuePair<short, ObjetoInteractivo> kvp in get_Interactivos_Utilizables(elementos))
            {
                if (get_Intentar_Mover_Interactivo(kvp))
                    return true;
            }

            cuenta.logger.log_Peligro("RECOLECCION", "No se han encontrado elementos recolectables");
            return false;
        }

        private Dictionary<short, ObjetoInteractivo> get_Interactivos_Utilizables(List<short> elementos_ids)
        {
            Dictionary<short, ObjetoInteractivo> elementos_utilizables = new Dictionary<short, ObjetoInteractivo>();

            foreach (Celda celda in mapa.celdas)
            {
                if (celda == null || celda.objeto_interactivo == null)
                    continue;

                if (!celda.objeto_interactivo.es_utilizable || !celda.objeto_interactivo.modelo.recolectable)
                    continue;

                foreach (short interactivo in celda.objeto_interactivo.modelo.habilidades)
                {
                    if (elementos_ids.Contains(interactivo))
                        elementos_utilizables.Add(celda.id, celda.objeto_interactivo);
                }
            }

            return elementos_utilizables;
        }

        private bool get_Intentar_Mover_Interactivo(KeyValuePair<short, ObjetoInteractivo> interactivo)
        {
            interactivo_recolectando = interactivo.Value;

             ResultadoMovimientos resultado = cuenta.juego.manejador.movimientos.get_Mover_A_Celda(interactivo_recolectando.celda, mapa.celdas_ocupadas, true);

            switch (resultado)
            {
                case ResultadoMovimientos.EXITO:
                case ResultadoMovimientos.MISMA_CELDA:
                    get_Intentar_Recolectar_Interactivo();
                return true;

                default:
                    get_Cancelar_Interactivo();
               return false;
            }
        }

        private void get_Intentar_Recolectar_Interactivo()
        {
            if (!robado)
            {
                foreach (short habilidad in interactivo_recolectando.modelo.habilidades)
                {
                    if (cuenta.juego.personaje.get_Skills_Recoleccion_Disponibles().Contains(habilidad))
                        cuenta.conexion.enviar_Paquete("GA500" + interactivo_recolectando.celda.id + ";" + habilidad);
                }
            }
            else
                evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, interactivo_recolectando.celda.id);
        }

        private void get_Movimiento_Finalizado(bool correcto)
        {
            if (interactivo_recolectando == null)
                return;

            if (!correcto && cuenta.juego.manejador.movimientos.actual_path != null)
                evento_Recoleccion_Acabada(RecoleccionResultado.FALLO, interactivo_recolectando.celda.id);
        }

        public async Task evento_Recoleccion_Iniciada(int id_personaje, int tiempo_delay, short celda_id, byte tipo_gkk)
        {
            if (interactivo_recolectando == null || interactivo_recolectando.celda.id != celda_id)
                return;

            if (cuenta.juego.personaje.id != id_personaje)
            {
                robado = true;
                cuenta.logger.log_informacion("INFORMACIÓN", "Un personaje te ha robado el recurso");
                evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, interactivo_recolectando.celda.id);
            }
            else
            {
                cuenta.Estado_Cuenta = EstadoCuenta.RECOLECTANDO;
                recoleccion_iniciada?.Invoke();
                await Task.Delay(tiempo_delay);
                cuenta.conexion.enviar_Paquete("GKK" + tipo_gkk);
            }
        }

        public void evento_Recoleccion_Acabada(RecoleccionResultado resultado, short celda_id)
        {
            if (interactivo_recolectando == null || interactivo_recolectando.celda.id != celda_id)
                return;

            robado = false;
            interactivo_recolectando = null;
            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
            recoleccion_acabada?.Invoke(resultado);
        }

        private void evento_Mapa_Actualizado()
        {
            pathfinder.set_Mapa(cuenta.juego.mapa);
            interactivos_no_utilizables.Clear();
        }

        public void limpiar()
        {
            interactivo_recolectando = null;
            interactivos_no_utilizables.Clear();
            robado = false;
        }

        #region Zona Dispose
        ~Recoleccion() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pathfinder.Dispose();
                }

                interactivos_no_utilizables.Clear();
                interactivos_no_utilizables = null;
                interactivo_recolectando = null;
                pathfinder = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
