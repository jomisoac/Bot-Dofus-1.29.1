using System;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class ManejadorHechizos : IDisposable
    {
        private Cuenta cuenta;
        private bool disposed;

        public ManejadorHechizos(Cuenta _cuenta)
        {
            cuenta = _cuenta;
        }

        public ResultadoLanzandoHechizo ManageSpell(HechizoPelea hechizo)
        {
            if (!hechizo.lanzar_cuerpo_cuerpo)
            {
                return get_Lanzar_Hechizo_Simple(hechizo);
            }
            else if (hechizo.lanzar_cuerpo_cuerpo && cuenta.pelea.esta_Cuerpo_A_Cuerpo_Con_Enemigo())
            {
                return get_Lanzar_Hechizo_Simple(hechizo);
            }
            return ResultadoLanzandoHechizo.NO_LANZADO;
        }

        private ResultadoLanzandoHechizo get_Lanzar_Hechizo_Simple(HechizoPelea spell)
        {
            if (cuenta.pelea.get_Puede_Lanzar_hechizo(spell.id) != FallosLanzandoHechizo.NINGUNO)
                return ResultadoLanzandoHechizo.NO_LANZADO;

            Luchadores enemigo = get_Objetivo_Mas_Cercano(spell);

            if (enemigo != null)
            {
                FallosLanzandoHechizo sir = cuenta.pelea.get_Puede_Lanzar_hechizo(spell.id, cuenta.pelea.jugador_luchador.celda_id, enemigo.celda_id, cuenta.personaje.mapa);

                if (sir == FallosLanzandoHechizo.NINGUNO)
                {
                    cuenta.pelea.get_Lanzar_Hechizo(spell.id, enemigo.celda_id);
                    return ResultadoLanzandoHechizo.LANZADO;
                }
                else if(sir == FallosLanzandoHechizo.NO_ESTA_EN_RANGO)
                {
                    return get_Mover_Lanzar_hechizo(spell, enemigo);
                }
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

        private ResultadoLanzandoHechizo get_Mover_Lanzar_hechizo(HechizoPelea hechizo, Luchadores enemigo)
        {
            Luchadores luchador = cuenta.pelea.get_Luchador_Por_Id(cuenta.personaje.id);

            if(luchador.pm > 0)
            {
                switch (cuenta.personaje.mapa.get_Mover_Celda_Resultado(luchador.celda_id, enemigo.celda_id, false, luchador.pm))
                {
                    case ResultadoMovimientos.EXITO:
                        luchador.pm = 0;
                    return ResultadoLanzandoHechizo.MOVIDO;

                    default:
                        return ResultadoLanzandoHechizo.NO_LANZADO;
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
