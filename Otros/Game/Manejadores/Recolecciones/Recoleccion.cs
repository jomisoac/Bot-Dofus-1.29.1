using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Game.Personaje;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario.Enums;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones
{
    public class Recoleccion : IDisposable
    {
        private Cuenta cuenta;
        private Mapa mapa;
        public ObjetoInteractivo interactivo_recolectando;
        private List<int> interactivos_no_utilizables;
        private bool robado;
        private Pathfinder pathfinder;
        private bool disposed;

        public event Action recoleccion_iniciada;
        public event Action<RecoleccionResultado> recoleccion_acabada;

        public static readonly int[] herramientas_pescar = { 8541, 6661, 596, 1866, 1865, 1864, 1867, 2188, 1863, 1862, 1868, 1861, 1860, 2366 };
        
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
            if (cuenta.esta_ocupado() || interactivo_recolectando != null)
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
            PersonajeJuego personaje = cuenta.juego.personaje;

            ObjetosInventario arma = personaje.inventario.get_Objeto_en_Posicion(InventarioPosiciones.ARMA);
            byte distancia_arma = 1;
            bool es_herramienta_pescar = false;

            if (arma != null)
            {
                distancia_arma = get_Distancia_herramienta(arma.id_modelo);
                es_herramienta_pescar = herramientas_pescar.Contains(arma.id_modelo);
            }

            foreach (ObjetoInteractivo interactivo in mapa.interactivos.Values)
            {
                if (!interactivo.es_utilizable || !interactivo.modelo.recolectable)
                    continue;

                List<Celda> path = pathfinder.get_Path(personaje.celda, interactivo.celda, mapa.celdas_ocupadas(), true, distancia_arma);

                if (path == null || path.Count == 0)
                    continue;

                foreach (short habilidad in interactivo.modelo.habilidades)
                {
                    if (!elementos_ids.Contains(habilidad))
                        continue;

                    if (!es_herramienta_pescar && path.Last().get_Distancia_Entre_Dos_Casillas(interactivo.celda) > 1)
                        continue;

                    if (es_herramienta_pescar && path.Last().get_Distancia_Entre_Dos_Casillas(interactivo.celda) > distancia_arma)
                        continue;

                    elementos_utilizables.Add(interactivo.celda.id, interactivo);
                }
            }
            
            return elementos_utilizables;
        }

        private bool get_Intentar_Mover_Interactivo(KeyValuePair<short, ObjetoInteractivo> interactivo)
        {
            interactivo_recolectando = interactivo.Value;
            byte distancia_detener = 1;
            ObjetosInventario arma = cuenta.juego.personaje.inventario.get_Objeto_en_Posicion(InventarioPosiciones.ARMA);

            if(arma != null)
                distancia_detener = get_Distancia_herramienta(arma.id_modelo);
            
            switch (cuenta.juego.manejador.movimientos.get_Mover_A_Celda(interactivo_recolectando.celda, mapa.celdas_ocupadas(), true, distancia_detener))
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

        public byte get_Distancia_herramienta(int id_objeto)
        {
            switch (id_objeto)
            {
                case 8541://Caña de aprendiz de pescador
                case 6661://Caña para kuakuás
                case 596://Caña de pescar corta
                    return 2;

                case 1866://Caña de pescar estándar
                    return 3;

                case 1865://Caña cúbica
                case 1864://La aguja de tejer
                    return 4;

                case 1867://Gran caña de pescar
                case 2188://Caña para pischis
                    return 5;

                case 1863://Caña Jillo
                case 1862://El Palo de Amor
                    return 6;

                case 1868:
                    return 7;

                case 1861://La Gran Perca
                case 1860://Caña arpón
                    return 8;

                case 2366://Caña para kralamares
                    return 9;
            }

            return 1;
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
