using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class ManejadorHechizos : IDisposable
    {
        private Cuenta cuenta;
        private bool disposed;

        public ManejadorHechizos(Cuenta _cuenta) => cuenta = _cuenta;

        public async Task<ResultadoLanzandoHechizo> manejador_Hechizos(HechizoPelea hechizo)
        {
            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return await lanzar_Hechizo_Celda_Vacia(hechizo);

            else if (!hechizo.lanzar_cuerpo_cuerpo)
                return await get_Lanzar_Hechizo_Simple(hechizo);

            else if (hechizo.lanzar_cuerpo_cuerpo && cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
                return await get_Lanzar_Hechizo_Simple(hechizo);

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> get_Lanzar_Hechizo_Simple(HechizoPelea spell)
        {
            if (cuenta.pelea.get_Puede_Lanzar_hechizo(spell.id) != FallosLanzandoHechizo.NINGUNO)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Luchadores enemigo = get_Objetivo_Mas_Cercano(spell);

            if (enemigo != null)
            {
                FallosLanzandoHechizo resultado = cuenta.pelea.get_Puede_Lanzar_hechizo(spell.id, enemigo.celda_id, cuenta.personaje.mapa);

                if (resultado == FallosLanzandoHechizo.NINGUNO)
                {
                    await cuenta.pelea.get_Lanzar_Hechizo(spell.id, enemigo.celda_id);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
                else if(resultado == FallosLanzandoHechizo.NO_ESTA_EN_RANGO)
                    return await get_Mover_Lanzar_hechizo(spell, enemigo);
            }
            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private Luchadores get_Objetivo_Mas_Cercano(HechizoPelea hechizo)
        {
            if (hechizo.focus == HechizoFocus.ENCIMA)
                return cuenta.pelea.jugador_luchador;

            if (hechizo.focus == HechizoFocus.CELDA_VACIA)
                return null;

            return hechizo.focus == HechizoFocus.ENEMIGO ? cuenta.pelea.get_Obtener_Enemigo_Mas_Cercano(cuenta.personaje.mapa) : cuenta.pelea.get_Obtener_Aliado_Mas_Cercano(cuenta.personaje.mapa);
        }

        private async Task<ResultadoLanzandoHechizo> get_Mover_Lanzar_hechizo(HechizoPelea hechizo_pelea, Luchadores enemigo)
        {
            Luchadores luchador = cuenta.pelea.get_Luchador_Por_Id(cuenta.personaje.id);
            Mapa mapa = cuenta.personaje.mapa;

            Hechizo hechizo_general = cuenta.personaje.hechizos.FirstOrDefault(f => f.id == hechizo_pelea.id);
            HechizoStats datos_hechizo = hechizo_general.get_Hechizo_Stats()[hechizo_general.nivel];

            int distancia_enemigo = mapa.get_Distancia_Entre_Dos_Casillas(luchador.celda_id, enemigo.celda_id);
            int pm_usados_lanzar_hechizo = distancia_enemigo >= datos_hechizo.alcanze_maximo ? luchador.pm : distancia_enemigo;
            
            if(luchador.pm > 0)
            {
                ResultadoMovimientos resultado = await cuenta.personaje.mapa.get_Mover_Celda_Resultado(luchador.celda_id, enemigo.celda_id, false, pm_usados_lanzar_hechizo);

                switch (resultado)
                {
                    case ResultadoMovimientos.EXITO:
                        return ResultadoLanzandoHechizo.MOVIDO;
                }
            }
            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        public async Task<ResultadoLanzandoHechizo> get_Mover(Luchadores enemigo)
        {
            Luchadores luchador = cuenta.pelea.get_Luchador_Por_Id(cuenta.personaje.id);

            if (luchador.pm > 0)
            {
                ResultadoMovimientos resultado = await cuenta.personaje.mapa.get_Mover_Celda_Resultado(luchador.celda_id, enemigo.celda_id, false, luchador.pm);

                switch (resultado)
                {
                    case ResultadoMovimientos.EXITO:
                    return ResultadoLanzandoHechizo.MOVIDO;
                }
            }
            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private async Task<ResultadoLanzandoHechizo> lanzar_Hechizo_Celda_Vacia(HechizoPelea hechizo_pelea)
        {
            if (cuenta.pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id) != FallosLanzandoHechizo.NINGUNO)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            if (hechizo_pelea.focus == HechizoFocus.CELDA_VACIA && cuenta.pelea.get_Cuerpo_A_Cuerpo_Enemigo().Count() == 4)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Hechizo hechizo_general = cuenta.personaje.hechizos.FirstOrDefault(f => f.id == hechizo_pelea.id);
            HechizoStats datos_hechizo = hechizo_general.get_Hechizo_Stats()[hechizo_general.nivel];

            List<int> rango = cuenta.pelea.get_Rango_hechizo(cuenta.pelea.jugador_luchador.celda_id, datos_hechizo, cuenta.personaje.mapa);
            for (int i = 0; i < rango.Count; i++)
            {
                if (cuenta.pelea.get_Puede_Lanzar_hechizo(hechizo_pelea.id, rango[i], cuenta.personaje.mapa) == FallosLanzandoHechizo.NINGUNO)
                {
                    if (hechizo_pelea.lanzar_cuerpo_cuerpo && cuenta.personaje.mapa.get_Distancia_Entre_Dos_Casillas(cuenta.pelea.jugador_luchador.celda_id, rango[i]) != 1)
                        continue;
                    
                    await cuenta.pelea.get_Lanzar_Hechizo(hechizo_pelea.id, rango[i]);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
            }

            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

            #region Zona Dispose
            ~ManejadorHechizos() => Dispose(false);

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
                }
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}
