using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Hechizos;
using Bot_Dofus_1._29._1.Otros.Entidades.Stats;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Protocolo.Enums;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes
{
    public class Personaje : Entidad
    {
        public int id { get; set; } = 0;
        public string nombre_personaje { get; set; } = string.Empty;
        public byte nivel { get; set; } = 0;
        public int gremio { get; set; } = 0;
        public byte sexo { get; set; } = 0;
        public int gfxID { get; set; } = 0;
        public int color1 { get; set; } = 0;
        public int color2 { get; set; } = 0;
        public int color3 { get; set; } = 0;
        public string objetos { get; set; } = string.Empty;
        public int puntos_caracteristicas { get; set; } = 0;
        public CaracteristicasInformacion caracteristicas { get; set; }
        public List<Hechizo> hechizos { get; set; }
        public string canales { get; set; } = string.Empty;
        public Mapa mapa;
        public int celda_id { get; set; } = 0;
        public bool esta_reconectando { get; set; } = false;
        private bool disposed;

        public int porcentaje_experiencia => (int)((caracteristicas.experiencia_actual - caracteristicas.experiencia_minima_nivel) / (caracteristicas.experiencia_siguiente_nivel - caracteristicas.experiencia_minima_nivel) * 100);

        public event Action personaje_seleccionado;
        public event Action socket_canal_personaje;
        public event Action caracteristicas_actualizadas;
        public event Action hechizos_actualizados;
        public event Action mapa_actualizado;
        public event Action<bool> movimiento_celda;
        public event Action<List<int>> movimiento_pathfinding;

        public Personaje(int _id, string _nombre_personaje, byte _nivel, int _gremio, byte _sexo, int _gfxID, int _color1, int _color2, int _color3, string _objetos)
        {
            id = _id;
            nombre_personaje = _nombre_personaje;
            nivel = _nivel;
            gremio = _gremio;
            sexo = _sexo;
            gfxID = _gfxID;
            color1 = _color1;
            color2 = _color2;
            color3 = _color3;
            objetos = _objetos;
            caracteristicas = new CaracteristicasInformacion();
            hechizos = new List<Hechizo>();
        }

        public Personaje(int _id, string _nombre_personaje, byte _sexo, int _celda_id)//Paquete GM+
        {
            id = _id;
            nombre_personaje = _nombre_personaje;
            sexo = _sexo;
            celda_id = _celda_id;
        }

        public void agregar_Canal_Personaje(string cadena_canales)
        {
            if (cadena_canales.Length <= 1)
            {
                canales += cadena_canales;
            }
            else
            {
                canales = cadena_canales;
                socket_canal_personaje?.Invoke();
            }
        }

        public void eliminar_Canal_Personaje(string simbolo_canal)
        {
            canales = canales.Replace(simbolo_canal, string.Empty);
            socket_canal_personaje?.Invoke();
        }

        #region Eventos
        public void evento_Mapa_Actualizado() => mapa_actualizado?.Invoke();
        public void evento_Personaje_Seleccionado() => personaje_seleccionado?.Invoke();
        public void evento_Personaje_Pathfinding(List<int> lista) => movimiento_pathfinding?.Invoke(lista);
        public void evento_Movimiento_Celda(bool resultado) => movimiento_celda?.Invoke(resultado);
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
            XElement xml = XElement.Parse(Properties.Resources.hechizos);
            string nombre;
            int hechizo_id;

            for (int i = 0; i < limitador.Length - 1; ++i)
            {
                separador = limitador[i].Split('~');
                hechizo_id = int.Parse(separador[0].ToString());
                nombre = xml.Elements("HECHIZO").Where(e => int.Parse(e.Element("id").Value) == hechizo_id).Elements("nombre").Select(e => e.Value).FirstOrDefault();
                hechizos.Add(new Hechizo(hechizo_id, nombre, byte.Parse(separador[1].ToString()), char.Parse(separador[2].ToString())));
            }
            xml = null;
            hechizos_actualizados.Invoke();
        }

        public Hechizo get_Hechizo(int id) => hechizos.FirstOrDefault(f => f.id == id);

        public async Task get_Manejar_Tiempo_Reconexion(Cuenta cuenta, int tiempo_espera)
        {
            if (esta_reconectando && tiempo_espera > 0)
            {
                cuenta.Estado_Cuenta = EstadoCuenta.LUCHANDO;
                cuenta.logger.log_informacion("RECONEXIÓN", "Reconexión detectada con tiempo esperando: " + tiempo_espera + " segundos");
                await Task.Delay(new TimeSpan(0, 0, tiempo_espera));
                esta_reconectando = false;
            }
        }

        ~Personaje() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if (disposing)
                {
                    if(mapa != null)
                        mapa.Dispose();
                }
                if (hechizos != null)
                    hechizos.Clear();
                mapa = null;
                hechizos = null;
                caracteristicas = null;
                nombre_personaje = null;
                objetos = null;
                disposed = true;
            }
        }
    }
}
