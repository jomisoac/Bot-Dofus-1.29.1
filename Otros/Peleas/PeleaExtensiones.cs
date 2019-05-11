using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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
        private int hechizo_lanzado_index;
        private bool esperando_sequencia_fin;
        private bool disposed;

        public PeleaExtensiones(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            configuracion = new PeleaConf(cuenta);
            manejador_hechizos = new ManejadorHechizos(cuenta);
            get_Eventos();
        }

        private void get_Eventos()
        {
            cuenta.pelea.pelea_creada += get_Pelea_Creada;
            cuenta.pelea.turno_iniciado += get_Pelea_Turno_iniciado;
            cuenta.pelea.hechizo_lanzado += get_Procesar_Despues_Accion;
            cuenta.pelea.movimiento_exito += get_Procesar_Despues_Accion;
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

            if (configuracion.hechizos.Count == 0 || !cuenta.pelea.get_Enemigos.Any())
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

                    if (GlobalConf.mostrar_mensajes_debug)
                        cuenta.logger.log_informacion("DEBUG", $"Hechizo {hechizo_actual.nombre} no lanzado");
                break;

                case ResultadoLanzandoHechizo.LANZADO:
                    hechizo_actual.lanzamientos_restantes--;
                    esperando_sequencia_fin = true;

                    if (GlobalConf.mostrar_mensajes_debug)
                        cuenta.logger.log_informacion("DEBUG", $"Hechizo {hechizo_actual.nombre} lanzado");
                break;

                case ResultadoLanzandoHechizo.MOVIDO:
                    esperando_sequencia_fin = true;

                    if (GlobalConf.mostrar_mensajes_debug)
                        cuenta.logger.log_informacion("DEBUG", $"Movimiento para intentar lanzar el hechizo {hechizo_actual.nombre} ");
                break;
            }
        }

        public async void get_Procesar_Despues_Accion()
        {
            if (cuenta.pelea.total_enemigos_vivos == 0)
                return;

            if (!esperando_sequencia_fin)
                return;

            esperando_sequencia_fin = false;
            await Task.Delay(400);
            await get_Procesar_hechizo();
        }

        private async Task get_Procesar_Siguiente_Hechizo(HechizoPelea hechizo_actual)
        {
            if (cuenta?.esta_luchando() == false)
                return;

            hechizo_actual.lanzamientos_restantes = hechizo_actual.lanzamientos_x_turno;
            hechizo_lanzado_index++;

            await get_Procesar_hechizo();
        }

        private async Task get_Fin_Turno()
        {
            if (!cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo() && configuracion.tactica == Tactica.AGRESIVA)
                await manejador_hechizos.get_Mover(cuenta.pelea.get_Obtener_Enemigo_Mas_Cercano(cuenta.personaje.mapa));

            cuenta.pelea.get_Turno_Acabado();
            cuenta.conexion.enviar_Paquete("Gt");
        }

        #region Zona Dispose
        ~PeleaExtensiones() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
