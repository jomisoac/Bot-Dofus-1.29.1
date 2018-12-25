using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas
{
    public class Mapa : IDisposable
    {
        public int id { get; set; } = 0;
        public int fecha { get; set; } = 0;
        public int anchura { get; set; } = 15;
        public int altura { get; set; } = 17;
        public string mapa_data { get; set; }
        public Celda[] celdas;
        private Cuenta cuenta { get; set; } = null;
        public bool verificar_Mapa_Actual(int mapa_id) => mapa_id == id;
        public Dictionary<int, Personaje> personajes;
        public Dictionary<int, List<Monstruo>> monstruos;//id, celda
        private bool disposed = false;

        private readonly XElement archivo_mapa;

        public Mapa(Cuenta _cuenta, string paquete)
        {
            get_Personajes().Clear();

            string[] _loc3 = paquete.Split('|');
            cuenta = _cuenta;
            id = int.Parse(_loc3[0]);
            fecha = int.Parse(_loc3[1]);

            archivo_mapa = XElement.Load("mapas/" + id + "_0" + fecha + ".xml");
            anchura = int.Parse(archivo_mapa.Element("ANCHURA").Value);
            altura = int.Parse(archivo_mapa.Element("ALTURA").Value);
            mapa_data = archivo_mapa.Element("MAPA_DATA").Value;

            archivo_mapa = null;//limpia la memoria
            descomprimir_mapa();
        }

        public ResultadoMovimientos get_Mover_Celda_Resultado(int celda_actual, int celda_destino, bool esquivar_monstruos)
        {
            if (cuenta.Estado_Cuenta != EstadoCuenta.CONECTADO_INACTIVO)
                return ResultadoMovimientos.FALLO;

            if (celda_destino < 0 || celda_destino > celdas.Length)
                return ResultadoMovimientos.FALLO;

            if (cuenta.esta_ocupado)
                return ResultadoMovimientos.FALLO;

            if (celda_actual == celda_destino)
                return ResultadoMovimientos.MISMA_CELDA;

            if (celdas[celda_destino].tipo == TipoCelda.NO_CAMINABLE)
                return ResultadoMovimientos.FALLO;
           
            Pathfinding camino = new Pathfinding(cuenta, cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO, esquivar_monstruos);
            if (camino.get_Camino(celda_actual, celda_destino))
            {
                cuenta.Estado_Cuenta = EstadoCuenta.MOVIMIENTO;
                cuenta.conexion.enviar_Paquete("GA001" + camino.get_Pathfinding_Limpio());

                Task.Delay(TimeSpan.FromMilliseconds(camino.get_Tiempo_Desplazamiento_Mapa(celda_actual, celda_destino))).Wait();
                cuenta.conexion.enviar_Paquete("GKK0");
                cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                return ResultadoMovimientos.EXITO;
            }
            return ResultadoMovimientos.FALLO;
        }

        public int get_Celda_Y_Coordenadas(int celda_id)
        {
            int loc5 = celda_id / ((anchura * 2) - 1);
            int loc6 = celda_id - (loc5 * ((anchura * 2) - 1));
            int loc7 = loc6 % anchura;
            return loc5 - loc7;
        }

        public int get_Celda_X_Coordenadas(int celda_id) => (celda_id - ((anchura - 1) * get_Celda_Y_Coordenadas(celda_id))) / anchura;


        public int get_Distancia_Entre_Dos_Casillas(int celda_1, int celda_2)
        {
            if (celda_1 != celda_2)
            {
                int diferencia_x = Math.Abs(get_Celda_X_Coordenadas(celda_1) - get_Celda_X_Coordenadas(celda_2));
                int diferencia_y = Math.Abs(get_Celda_Y_Coordenadas(celda_1) - get_Celda_Y_Coordenadas(celda_2));
                return diferencia_x + diferencia_y;
            }
            else
                return 0;
        }

        #region Metodos de descompresion
        public void descomprimir_mapa()
        {
            celdas = new Celda[mapa_data.Length / 10];
            string celda_data;

            for (int i = 0; i < mapa_data.Length; i += 10)
            {
                celda_data = mapa_data.Substring(i, 10);
                celdas[i / 10] = descompimir_Celda(celda_data, i / 10);
            }
        }

        public Celda descompimir_Celda(string celda_data, int id_celda)
        {
            Celda celda = new Celda(id_celda);

            byte[] informacion_celda = new byte[celda_data.Length];
            for (int i = 0; i < celda_data.Length; i++)
            {
                informacion_celda[i] = Convert.ToByte(Compressor.index_Hash(celda_data[i]));
            }

            celda.tipo = (TipoCelda)((informacion_celda[2] & 56) >> 3);
            celda.es_linea_vision = (informacion_celda[0] & 1) != 0;
            celda.objeto_interactivo_id = ((informacion_celda[7] & 2) >> 1) != 0 ? ((informacion_celda[0] & 2) << 12) + ((informacion_celda[7] & 1) << 12) + (informacion_celda[8] << 6) + informacion_celda[9] : -1;
            celda.object2Movement = ((informacion_celda[7] & 2) >> 1) != 0;
            celda.layerGroundLevel = Convert.ToByte(informacion_celda[1] & 15);
            celda.layerGroundSlope = Convert.ToByte((informacion_celda[4] & 60) >> 2);
            return celda;
        }
        #endregion

        #region metodos monstruos
        public void agregar_Monstruo(Monstruo monstruo)
        {
            if (monstruos == null)
                monstruos = new Dictionary<int, List<Monstruo>>();
            if (monstruos.ContainsKey(monstruo.id))
                monstruos[monstruo.id].Add(monstruo);
            else
                monstruos.Add(monstruo.id, new List<Monstruo> { monstruo });
        }

        public void eliminar_Monstruo(int id)
        {
            if (monstruos != null)
            {
                if (monstruos.ContainsKey(id))
                    monstruos.Remove(id);
                if (monstruos.Count <= 0)//si no tiene ninguno volvera a ser nulo
                    monstruos = null;
            }
        }

        public Dictionary<int, List<Monstruo>> get_Monstruos()
        {
            if (monstruos == null)
                return new Dictionary<int, List<Monstruo>> ();
            return monstruos;
        }
        #endregion

        #region metodos personajes
        public void agregar_Personaje(Personaje personaje)
        {
            if (personajes == null)
                personajes = new Dictionary<int, Personaje>();
            if (!personajes.ContainsKey(personaje.id))
                personajes.Add(personaje.id, personaje);
        }

        public void eliminar_Personaje(int id)
        {
            if (personajes != null)
            {
                if (personajes.ContainsKey(id))
                    personajes.Remove(id);
                if (personajes.Count <= 0)//si no tiene ninguno volvera a ser nulo
                    personajes = null;
            }
        }

        public Dictionary<int, Personaje> get_Personajes()
        {
            if (personajes == null)
                return new Dictionary<int, Personaje>();
            return personajes;
        }
        #endregion

        #region Zona Dispose
        ~Mapa() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (personajes != null)
                    personajes.Clear();
                if (monstruos != null)
                    monstruos.Clear();
                celdas = null;
                personajes = null;
                cuenta = null;
                mapa_data = null;
                disposed = true;
            }
        }
        #endregion
    }
}
