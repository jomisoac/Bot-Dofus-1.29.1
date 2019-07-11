using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Oficios;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje
{
    public class PersonajeJuego : Entidad, IEliminable
    {
        public int id { get; set; } = 0;
        public string nombre { get; set; }
        public byte nivel { get; set; } = 0;
        public byte sexo { get; set; } = 0;
        public byte raza_id { get; set; } = 0;
        private Cuenta cuenta;
        public InventarioGeneral inventario { get; private set; }
        public int puntos_caracteristicas { get; set; } = 0;
        public int kamas { get; private set; } = 0;
        public Caracteristicas caracteristicas { get; set; }
        public Dictionary<short, Hechizo> hechizos { get; set; }//id_hechizo, hechizo
        public List<Oficio> oficios { get; private set; }
        public Timer timer_regeneracion { get; private set; }
        public Timer timer_afk { get; private set; }
        public string canales { get; set; } = string.Empty;
        public Celda celda { get; set; }
        
        public bool en_grupo { get; set; } = false;
        private bool disposed;

        /** Estados **/
        public bool esta_utilizando_dragopavo { get; set; } = false;
        public sbyte hablando_npc_id { get; set; }

        public int porcentaje_experiencia => (int)((caracteristicas.experiencia_actual - caracteristicas.experiencia_minima_nivel) / (caracteristicas.experiencia_siguiente_nivel - caracteristicas.experiencia_minima_nivel) * 100);

        /** Eventos **/
        public event Action servidor_seleccionado;
        public event Action personaje_seleccionado;
        public event Action caracteristicas_actualizadas;
        public event Action pods_actualizados;
        public event Action hechizos_actualizados;
        public event Action oficios_actualizados;
        public event Action dialogo_npc_recibido;
        public event Action dialogo_npc_acabado;
        public event Action<List<Celda>> movimiento_pathfinding_minimapa;
        
        public PersonajeJuego(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            timer_regeneracion = new Timer(regeneracion_TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            timer_afk = new Timer(anti_Afk, null, Timeout.Infinite, Timeout.Infinite);//1200000

            inventario = new InventarioGeneral(cuenta);
            caracteristicas = new Caracteristicas();
            hechizos = new Dictionary<short, Hechizo>();
            oficios = new List<Oficio>();
        }

        public void set_Datos_Personaje(int _id, string _nombre_personaje, byte _nivel, byte _sexo, byte _raza_id)
        {
            id = _id;
            nombre = _nombre_personaje;
            nivel = _nivel;
            sexo = _sexo;
            raza_id = _raza_id;
        }

        public void agregar_Canal_Personaje(string cadena_canales)
        {
            if (cadena_canales.Length <= 1)
                canales += cadena_canales;
            else
                canales = cadena_canales;
        }

        public void eliminar_Canal_Personaje(string simbolo_canal) => canales = canales.Replace(simbolo_canal, string.Empty);

        #region Eventos
        public void evento_Pods_Actualizados() => pods_actualizados?.Invoke();
        public void evento_Servidor_Seleccionado() => servidor_seleccionado?.Invoke();
        public void evento_Personaje_Seleccionado() => personaje_seleccionado?.Invoke();
        public void evento_Personaje_Pathfinding_Minimapa(List<Celda> lista) => movimiento_pathfinding_minimapa?.Invoke(lista);
        public void evento_Oficios_Actualizados() => oficios_actualizados?.Invoke();
        public void evento_Dialogo_Recibido() => dialogo_npc_recibido?.Invoke();
        public void evento_Dialogo_Acabado() => dialogo_npc_acabado?.Invoke();
        #endregion

        public void actualizar_Caracteristicas(string paquete)
        {
            string[] _loc3 = paquete.Substring(2).Split('|');
            string[] _loc5 = _loc3[0].Split(',');

            caracteristicas.experiencia_actual = double.Parse(_loc5[0]);
            caracteristicas.experiencia_minima_nivel = double.Parse(_loc5[1]);
            caracteristicas.experiencia_siguiente_nivel = double.Parse(_loc5[2]);
            kamas = int.Parse(_loc3[1]);
            puntos_caracteristicas = int.Parse(_loc3[2]);

            _loc5 = _loc3[5].Split(',');
            caracteristicas.vitalidad_actual = int.Parse(_loc5[0]);
            caracteristicas.vitalidad_maxima = int.Parse(_loc5[1]);

            _loc5 = _loc3[6].Split(',');
            caracteristicas.energia_actual = int.Parse(_loc5[0]);
            caracteristicas.maxima_energia = int.Parse(_loc5[1]);

            if (caracteristicas.iniciativa != null)
                caracteristicas.iniciativa.base_personaje = int.Parse(_loc3[7]);
            else
                caracteristicas.iniciativa = new PersonajeStats(int.Parse(_loc3[7]));

            if (caracteristicas.prospeccion != null)
                caracteristicas.prospeccion.base_personaje = int.Parse(_loc3[8]);
            else
                caracteristicas.prospeccion = new PersonajeStats(int.Parse(_loc3[8]));

            for (int i = 9; i <= 18; ++i)
            {
                _loc5 = _loc3[i].Split(',');
                int base_personaje = int.Parse(_loc5[0]);
                int equipamiento = int.Parse(_loc5[1]);
                int dones = int.Parse(_loc5[2]);
                int boost = int.Parse(_loc5[3]);
                
                switch (i)
                {
                    case 9:
                        caracteristicas.puntos_accion.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 10:
                        caracteristicas.puntos_movimiento.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                   break;

                    case 11:
                        caracteristicas.fuerza.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 12:
                        caracteristicas.vitalidad.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 13:
                        caracteristicas.sabiduria.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 14:
                        caracteristicas.suerte.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 15:
                        caracteristicas.agilidad.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 16:
                        caracteristicas.inteligencia.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 17:
                        caracteristicas.alcanze.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;

                    case 18:
                        caracteristicas.criaturas_invocables.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                    break;
                }
            }
            caracteristicas_actualizadas?.Invoke();
        }

        public void actualizar_Hechizos(string paquete)
        {
            hechizos.Clear();

            string[] limitador = paquete.Split(';'), separador;
            Hechizo hechizo;
            short hechizo_id;

            for (int i = 0; i < limitador.Length - 1; ++i)
            {
                separador = limitador[i].Split('~');
                hechizo_id = short.Parse(separador[0]);

                hechizo = Hechizo.get_Hechizo(hechizo_id);
                hechizo.nivel = byte.Parse(separador[1]);

                hechizos.Add(hechizo_id, hechizo);
            }
            hechizos_actualizados.Invoke();
        }

        private void regeneracion_TimerCallback(object state)
        {
            try
            {
                if (caracteristicas?.vitalidad_actual >= caracteristicas?.vitalidad_maxima)
                {
                    timer_regeneracion.Change(Timeout.Infinite, Timeout.Infinite);
                    return;
                }

                caracteristicas.vitalidad_actual++;
                caracteristicas_actualizadas?.Invoke();
            }
            catch (Exception e)
            {
                cuenta.logger.log_Error("TIMER-REGENERANDO", $"ERROR: {e}");
            }
        }

        private void anti_Afk(object state)
        {
            try
            {
                if(cuenta.Estado_Cuenta != EstadoCuenta.DESCONECTADO)
                    cuenta.conexion.enviar_Paquete("ping");
            }
            catch (Exception e)
            {
                cuenta.logger.log_Error("TIMER-ANTIAFK", $"ERROR: {e}");
            }
        }

        public Hechizo get_Hechizo(short id) => hechizos[id];
        public bool get_Tiene_Skill_Id(int id) => oficios.FirstOrDefault(j => j.skills.FirstOrDefault(s => s.id == id) != null) != null;
        public IEnumerable<SkillsOficio> get_Skills_Disponibles() => oficios.SelectMany(oficio => oficio.skills.Select(skill => skill));
        public IEnumerable<short> get_Skills_Recoleccion_Disponibles() => oficios.SelectMany(oficio => oficio.skills.Where(skill => !skill.es_craft).Select(skill => skill.id));

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~PersonajeJuego() => Dispose(false);

        public void limpiar()
        {
            id = 0;
            hechizos.Clear();
            oficios.Clear();
            inventario.limpiar();
            caracteristicas.limpiar();

            timer_regeneracion.Change(Timeout.Infinite, Timeout.Infinite);
            timer_afk.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if (disposing)
                {
                    inventario.Dispose();
                    timer_regeneracion.Dispose();
                    timer_afk.Dispose();
                }
                
                hechizos = null;
                caracteristicas = null;
                nombre = null;
                inventario = null;
                timer_regeneracion = null;
                timer_afk = null;
                disposed = true;
            }
        }
        #endregion
    }
}
