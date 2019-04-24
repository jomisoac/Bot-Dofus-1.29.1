using System;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

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
        }

        private void get_Pelea_Creada()
        {
            configuracion.hechizos.ForEach(s =>
            {
                s.lanzamientos_restantes = s.lanzamientos_x_turno;
            });
        }

        private async void get_Pelea_Turno_iniciado()
        {
            hechizo_lanzado_index = 0;

            if (configuracion.hechizos.Count == 0)
            {
                await get_Fin_Turno();
                return;
            }

            await get_Procesar_hechizo();
        }

        private async Task get_Procesar_hechizo()
        {
            if (cuenta.esta_luchando() || configuracion != null)
            {
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
                else
                {
                    switch (await manejador_hechizos.manejador_Hechizos(hechizo_actual))
                    {
                        case ResultadoLanzandoHechizo.NO_LANZADO:
                            await get_Procesar_Siguiente_Hechizo(hechizo_actual);
                        break;

                        case ResultadoLanzandoHechizo.LANZADO:
                            if (GlobalConf.mostrar_mensajes_debug)
                                cuenta.logger.log_informacion("DEBUG", $"Hechizo {hechizo_actual.nombre} lanzado");
                            hechizo_actual.lanzamientos_restantes--;
                            await get_Procesar_hechizo();
                        break;

                        case ResultadoLanzandoHechizo.MOVIDO:
                            await get_Procesar_hechizo();
                        break;
                    }
                }
            }
        }
        private async Task get_Procesar_Siguiente_Hechizo(HechizoPelea hechizo_actual)
        {
            if (!cuenta.esta_luchando())
                return;

            hechizo_actual.lanzamientos_restantes = hechizo_actual.lanzamientos_x_turno;
            hechizo_lanzado_index++;

            await get_Procesar_hechizo();
        }

        private async Task get_Fin_Turno()
        {
            if (configuracion.tactica == Tactica.PASIVA)
            {
                await get_Acabar_Turno();
                return;
            }
            if (cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo() && configuracion.tactica == Tactica.AGRESIVA)
            {
                await get_Acabar_Turno();
                return;
            }
            await get_Acabar_Turno();
            return;
        }

        private async Task get_Acabar_Turno()
        {
            cuenta.pelea.get_Turno_Acabado();
            await cuenta.conexion.enviar_Paquete("Gt");
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
