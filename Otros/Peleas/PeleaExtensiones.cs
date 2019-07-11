using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
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

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class PeleaExtensiones : IDisposable
    {
        public PeleaConf configuracion { get; set; }
        private Cuenta cuenta;
        private ManejadorHechizos manejador_hechizos;
        private Pelea pelea;

        private int hechizo_lanzado_index;
        private bool esperando_sequencia_fin;
        private bool disposed;

        public PeleaExtensiones(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            configuracion = new PeleaConf(cuenta);
            manejador_hechizos = new ManejadorHechizos(cuenta);
            pelea = cuenta.juego.pelea;

            get_Eventos();
        }

        private void get_Eventos()
        {
            pelea.pelea_creada += get_Pelea_Creada;
            pelea.turno_iniciado += get_Pelea_Turno_iniciado;
            pelea.hechizo_lanzado += get_Procesar_Hechizo_Lanzado;
            pelea.movimiento += get_Procesar_Movimiento;
        }

        private void get_Pelea_Creada()
        {
            foreach (HechizoPelea hechizo in configuracion.hechizos)
                hechizo.lanzamientos_restantes = hechizo.lanzamientos_x_turno;
        }

        private async void get_Pelea_Turno_iniciado()
        {
            hechizo_lanzado_index = 0;
            esperando_sequencia_fin = true;

            await Task.Delay(400);

            if (configuracion.hechizos.Count == 0 || !cuenta.juego.pelea.get_Enemigos.Any())
            {
                await get_Fin_Turno();
                return;
            }
            await get_Procesar_hechizo();
        }

        private async Task get_Procesar_hechizo()
        {
            if (cuenta?.esta_luchando() == false || configuracion == null)
                return;

            if (hechizo_lanzado_index >= configuracion.hechizos.Count)
            {
                await get_Fin_Turno();
                return;
            }

            HechizoPelea hechizo_actual = configuracion.hechizos[hechizo_lanzado_index];
            
            if (hechizo_actual.lanzamientos_restantes == 0)
            {
                await get_Procesar_Siguiente_Hechizo(hechizo_actual);
                return;
            }

            ResultadoLanzandoHechizo resultado = await manejador_hechizos.manejador_Hechizos(hechizo_actual);
            switch (resultado)
            {
                case ResultadoLanzandoHechizo.NO_LANZADO:
                    await get_Procesar_Siguiente_Hechizo(hechizo_actual);
                break;

                case ResultadoLanzandoHechizo.LANZADO:
                    hechizo_actual.lanzamientos_restantes--;
                    esperando_sequencia_fin = true;
                break;

                case ResultadoLanzandoHechizo.MOVIDO:
                    esperando_sequencia_fin = true;
                break;
            }
        }

        public async void get_Procesar_Hechizo_Lanzado(short celda_id, bool exito)
        {
            if (pelea.total_enemigos_vivos == 0)
                return;

            if (!esperando_sequencia_fin)
                return;

            esperando_sequencia_fin = false;
            await Task.Delay(400);

            if (!exito)
            {
                await get_Procesar_Siguiente_Hechizo(configuracion.hechizos[hechizo_lanzado_index]);
                return;
            }

            pelea.actualizar_Hechizo_Exito(celda_id, configuracion.hechizos[hechizo_lanzado_index].id);
            await get_Procesar_hechizo();
        }

        public async void get_Procesar_Movimiento(bool exito)
        {
            if (pelea.total_enemigos_vivos == 0)
                return;

            if (!esperando_sequencia_fin)
                return;

            esperando_sequencia_fin = false;
            await Task.Delay(400);

            if (!exito)
            {
                await get_Procesar_Siguiente_Hechizo(configuracion.hechizos[hechizo_lanzado_index]);
                return;
            }

            await get_Procesar_hechizo();
        }

        private async Task get_Procesar_Siguiente_Hechizo(HechizoPelea hechizo_actual)
        {
            if (cuenta?.esta_luchando() == false)
                return;

            hechizo_actual.lanzamientos_restantes = hechizo_actual.lanzamientos_x_turno;
            hechizo_lanzado_index++;

            await Task.Delay(350 + cuenta.conexion.get_Actual_Ping());
            await get_Procesar_hechizo();
        }

        private async Task get_Fin_Turno()
        {
            if (!pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo() && configuracion.tactica == Tactica.AGRESIVA)
                await get_Mover(true, pelea.get_Obtener_Enemigo_Mas_Cercano());
            else if (pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo() && configuracion.tactica == Tactica.FUGITIVA)
                await get_Mover(false, pelea.get_Obtener_Enemigo_Mas_Cercano());

            pelea.get_Turno_Acabado();
            cuenta.conexion.enviar_Paquete("Gt");
        }

        public async Task get_Mover(bool cercano, Luchadores enemigo)
        {
            KeyValuePair<short, MovimientoNodo>? nodo = null;
            Mapa mapa = cuenta.juego.mapa;
            int distancia = -1;

            int distancia_total = Get_Total_Distancia_Enemigo(pelea.jugador_luchador.celda);

            foreach (KeyValuePair<short, MovimientoNodo> kvp in PeleasPathfinder.get_Celdas_Accesibles(pelea, mapa, pelea.jugador_luchador.celda))
            {
                if (!kvp.Value.alcanzable)
                    continue;

                int temporal_distancia = Get_Total_Distancia_Enemigo(mapa.get_Celda_Id(kvp.Key));

                if ((cercano && temporal_distancia <= distancia_total) || (!cercano && temporal_distancia >= distancia_total))
                {
                    if (cercano)
                    {
                        nodo = kvp;
                        distancia_total = temporal_distancia;
                    }
                    else if (kvp.Value.camino.celdas_accesibles.Count >= distancia)
                    {
                        nodo = kvp;
                        distancia_total = temporal_distancia;
                        distancia = kvp.Value.camino.celdas_accesibles.Count;
                    }
                }
            }

            if (nodo != null)
                await cuenta.juego.manejador.movimientos.get_Mover_Celda_Pelea(nodo);
        }

        public int Get_Total_Distancia_Enemigo(Celda celda) => cuenta.juego.pelea.get_Enemigos.Sum(e => e.celda.get_Distancia_Entre_Dos_Casillas(celda) - 1);

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~PeleaExtensiones() => Dispose(false);
        
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    configuracion.Dispose();
                }
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
