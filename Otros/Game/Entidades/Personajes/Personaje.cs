using System;
using System.Collections.Generic;
using System.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Inventario;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes.Oficios;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Stats;
using Bot_Dofus_1._29._1.Otros.Mapas;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes
{
    public class Personaje : Entidad
    {
        public int id { get; set; } = 0;
        public string nombre_personaje { get; set; } = string.Empty;
        public byte nivel { get; set; } = 0;
        public byte sexo { get; set; } = 0;
        public byte raza_id { get; set; } = 0;
        public InventarioGeneral inventario { get; private set; }
        public int puntos_caracteristicas { get; set; } = 0;
        public CaracteristicasInformacion caracteristicas { get; set; }
        public List<Hechizo> hechizos { get; set; }
        public List<Oficio> oficios { get; private set; }
        public string canales { get; set; } = string.Empty;
        public Celda celda { get; set; }
        
        public bool en_grupo { get; set; } = false;
        private bool disposed;

        /** Estados **/
        public bool esta_utilizando_dragopavo { get; set; } = false;


        public int porcentaje_experiencia => (int)((caracteristicas.experiencia_actual - caracteristicas.experiencia_minima_nivel) / (caracteristicas.experiencia_siguiente_nivel - caracteristicas.experiencia_minima_nivel) * 100);

        /** Eventos **/
        public event Action personaje_seleccionado;
        public event Action caracteristicas_actualizadas;
        public event Action pods_actualizados;
        public event Action hechizos_actualizados;
        public event Action oficios_actualizados;
        public event Action<string> pregunta_npc_recibida;
        public event Action<List<short>> movimiento_pathfinding_minimapa;
        
        public Personaje(Cuenta cuenta)
        {
            inventario = new InventarioGeneral(cuenta);
            caracteristicas = new CaracteristicasInformacion();
            hechizos = new List<Hechizo>();
            oficios = new List<Oficio>();
        }

        public Personaje(int _id, string _nombre_personaje, byte _sexo, Celda _celda)//Paquete GM+
        {
            id = _id;
            nombre_personaje = _nombre_personaje;
            sexo = _sexo;
            celda = _celda;
        }

        public void set_Datos_Personaje(int _id, string _nombre_personaje, byte _nivel, byte _sexo, byte _raza_id)
        {
            id = _id;
            nombre_personaje = _nombre_personaje;
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

        public void eliminar_Canal_Personaje(string simbolo_canal)
        {
            canales = canales.Replace(simbolo_canal, string.Empty);
        }

        #region Eventos
        public void evento_Pods_Actualizados() => pods_actualizados?.Invoke();
        public void evento_Personaje_Seleccionado() => personaje_seleccionado?.Invoke();
        public void evento_Personaje_Pathfinding_Minimapa(List<short> lista) => movimiento_pathfinding_minimapa?.Invoke(lista);
        public void evento_Oficios_Actualizados() => oficios_actualizados?.Invoke();
        public void evento_Pregunta_Npc(string lista_preguntas) => pregunta_npc_recibida?.Invoke(lista_preguntas);
        #endregion

        public void actualizar_Caracteristicas(string paquete)
        {
            string[] _loc3 = paquete.Substring(2).Split('|');
            string[] _loc5 = _loc3[0].Split(',');

            caracteristicas.experiencia_actual = double.Parse(_loc5[0]);
            caracteristicas.experiencia_minima_nivel = double.Parse(_loc5[1]);
            caracteristicas.experiencia_siguiente_nivel = double.Parse(_loc5[2]);
            caracteristicas.kamas = int.Parse(_loc3[1]);
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
                caracteristicas.iniciativa = new CaracteristicasBase(int.Parse(_loc3[7]));

            if (caracteristicas.prospeccion != null)
                caracteristicas.prospeccion.base_personaje = int.Parse(_loc3[8]);
            else
                caracteristicas.prospeccion = new CaracteristicasBase(int.Parse(_loc3[8]));

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
                        if (caracteristicas.puntos_accion != null)
                            caracteristicas.puntos_accion.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.puntos_accion = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 10:
                        if (caracteristicas.puntos_movimiento != null)
                            caracteristicas.puntos_movimiento.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.puntos_movimiento = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 11:
                        if (caracteristicas.fuerza != null)
                            caracteristicas.fuerza.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.fuerza = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 12:
                        if (caracteristicas.vitalidad != null)
                            caracteristicas.vitalidad.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.vitalidad = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 13:
                        if (caracteristicas.sabiduria != null)
                            caracteristicas.sabiduria.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.sabiduria = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 14:
                        if (caracteristicas.suerte != null)
                            caracteristicas.suerte.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.suerte = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 15:
                        if (caracteristicas.agilidad != null)
                            caracteristicas.agilidad.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.agilidad = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 16:
                        if (caracteristicas.inteligencia != null)
                            caracteristicas.inteligencia.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.inteligencia = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 17:
                        if (caracteristicas.alcanze != null)
                            caracteristicas.alcanze.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.alcanze = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;

                    case 18:
                        if (caracteristicas.criaturas_invocables != null)
                            caracteristicas.criaturas_invocables.actualizar_Stats(base_personaje, equipamiento, dones, boost);
                        else
                            caracteristicas.criaturas_invocables = new CaracteristicasBase(base_personaje, equipamiento, dones, boost);
                    break;
                }
            }
            caracteristicas_actualizadas?.Invoke();
        }

        public void actualizar_Hechizos(string paquete)
        {
            hechizos.Clear();

            string[] limitador = paquete.Split(';'), separador;
            short hechizo_id;
            byte nivel;

            for (int i = 0; i < limitador.Length - 1; ++i)
            {
                separador = limitador[i].Split('~');
                hechizo_id = short.Parse(separador[0]);
                nivel = byte.Parse(separador[1]);

                hechizos.Add(Hechizo.get_Hechizo(hechizo_id, nivel));
            }
            hechizos_actualizados.Invoke();
        }

        public Hechizo get_Hechizo(int id) => hechizos.FirstOrDefault(f => f.id == id);
        public bool get_Tiene_Skill_Id(int id) => oficios.FirstOrDefault(j => j.skills.FirstOrDefault(s => s.id == id) != null) != null;
        public IEnumerable<SkillsOficio> get_Skills_Disponibles() => oficios.SelectMany(oficio => oficio.skills.Select(skill => skill));
        public IEnumerable<short> get_Skills_Recoleccion_Disponibles() => oficios.SelectMany(oficio => oficio.skills.Where(skill => !skill.es_craft).Select(skill => skill.id));

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Personaje() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if (disposing)
                {
                    inventario.Dispose();
                }
                
                hechizos = null;
                caracteristicas = null;
                nombre_personaje = null;
                inventario = null;
                disposed = true;
            }
        }
        #endregion
    }
}
