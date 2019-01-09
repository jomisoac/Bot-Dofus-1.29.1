using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
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
        public Dictionary<int, Monstruo> monstruos;//id, celda
        private bool disposed = false;

        private readonly XElement archivo_mapa;

        public Mapa(Cuenta _cuenta, string paquete)
        {
            get_Personajes().Clear();
            get_Monstruos().Clear();

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

        public Celda get_Celda_Id(int celda_id) => celdas[celda_id];

        public ResultadoMovimientos get_Mover_Celda_Resultado(int celda_actual, int celda_destino, bool esquivar_monstruos, int pm_pelea = 3)
        {
            if (celda_destino < 0 || celda_destino > celdas.Length)
                return ResultadoMovimientos.FALLO;

            bool esta_en_pelea = cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO;

            if (!esta_en_pelea)
            {
                if (cuenta.esta_ocupado)
                    return ResultadoMovimientos.FALLO;
            }

            if (celda_actual == celda_destino)
                return ResultadoMovimientos.MISMA_CELDA;

            if (celdas[celda_destino].tipo == TipoCelda.NO_CAMINABLE)
                return ResultadoMovimientos.FALLO;

            Pathfinding camino = new Pathfinding(cuenta, esta_en_pelea, esquivar_monstruos, pm_pelea);
            if (camino.get_Camino(celda_actual, celda_destino))
            {
                if (!esta_en_pelea)
                    cuenta.Estado_Cuenta = EstadoCuenta.MOVIMIENTO;
                cuenta.conexion.enviar_Paquete("GA001" + camino.get_Pathfinding_Limpio());

                Task.Delay(TimeSpan.FromMilliseconds(camino.get_Tiempo_Desplazamiento_Mapa(celda_actual, celda_destino))).Wait();

                if (cuenta.Estado_Cuenta != EstadoCuenta.DESCONECTADO)
                {
                    cuenta.conexion.enviar_Paquete("GKK0");
                    cuenta.personaje.evento_Movimiento_Celda(true);

                    if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                        cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;

                    return ResultadoMovimientos.EXITO;
                }
            }
            cuenta.personaje.evento_Movimiento_Celda(false);
            return ResultadoMovimientos.FALLO;
        }

        public bool get_Puede_Luchar_Contra_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios) => get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, nivel_minimo, nivel_maximo, monstruos_prohibidos, monstruos_obligatorios).Count > 0;

        public List<Monstruo> get_Grupo_Monstruos(int monstruos_minimos, int monstruos_maximos, int nivel_minimo, int nivel_maximo, List<int> monstruos_prohibidos, List<int> monstruos_obligatorios)
        {
            List<Monstruo> grupos_monstruos_disponibles = new List<Monstruo>();

            foreach (Monstruo grupo_monstruos in get_Monstruos().Values)
            {
                if (grupo_monstruos.get_Total_Monstruos < monstruos_minimos || grupo_monstruos.get_Total_Monstruos > monstruos_maximos)
                    continue;

                if (grupo_monstruos.get_Total_Nivel_Grupo < nivel_minimo || grupo_monstruos.get_Total_Nivel_Grupo > nivel_maximo)
                    continue;

                bool es_valido = true;

                if (monstruos_prohibidos != null)
                {
                    for (int i = 0; i < monstruos_prohibidos.Count; i++)
                    {
                        if (grupo_monstruos.get_Contiene_Monstruo(monstruos_prohibidos[i]))
                        {
                            es_valido = false;
                            break;
                        }
                    }
                }

                if (monstruos_obligatorios != null && es_valido)
                {
                    for (int i = 0; i < monstruos_obligatorios.Count; i++)
                    {
                        if (!grupo_monstruos.get_Contiene_Monstruo(monstruos_obligatorios[i]))
                        {
                            es_valido = false;
                            break;
                        }
                    }
                }

                if (es_valido)
                {
                    grupos_monstruos_disponibles.Add(grupo_monstruos);
                }
            }
            return grupos_monstruos_disponibles;
        }

        public bool get_Verificar_Linea_Vision(int cell1, int cell2)
        {
            int dist = get_Distancia_Entre_Dos_Casillas(cell1, cell2);
            List<int> los = new List<int>();
            if (dist > 2)
                los = get_Linea_Vision(cell1, cell2);
            if (los != null && dist > 2)
            {
                foreach (int i in los)
                {
                    if (i != cell1 && i != cell2 && !celdas[i].es_linea_vision)
                        return false;
                }
            }
            if (dist > 2)
            {
                int cell = get_Obtener_Celda_Cercana(cell2, cell1);
                if (cell != -1 && !celdas[cell].es_linea_vision)
                    return false;
            }
            return true;
        }

        public int get_Obtener_Celda_Cercana(int celda_inicial, int celda_final)
        {
            int dist = 1000;
            int celda_id = celda_inicial;
            char[] dirs = { 'b', 'd', 'f', 'h' };
            foreach (char d in dirs)
            {
                int celda = get_Celda_Desde_Direccion(celda_inicial, d);
                int dis = get_Distancia_Entre_Dos_Casillas(celda_final, celda);
                if (dis < dist && celdas[celda].tipo == TipoCelda.CELDA_CAMINABLE && cuenta.pelea.es_Celda_Libre(celda))
                {
                    dist = dis;
                    celda_id = celda;
                }
            }
            return celda_id == celda_inicial ? -1 : celda_id;
        }

        public int get_Celda_Desde_Direccion(int celda_id, char direccion, int distancia = 1)
        {
            switch (direccion)
            {
                case 'a':
                    return celda_id + distancia;
                case 'b':
                    return celda_id + anchura;
                case 'c':
                    return celda_id + (anchura * 2 - distancia);
                case 'd':
                    return celda_id + (anchura - 1);
                case 'e':
                    return celda_id - distancia;
                case 'f':
                    return celda_id - anchura;
                case 'g':
                    return celda_id - (anchura * 2 - distancia);
                case 'h':
                    return celda_id - anchura + 1;
            }
            return -1;
        }

        public List<int> get_Linea_Vision(int celda_1, int celda_2)
        {
            List<int> Los = new List<int>();
            int celda_id = celda_1;
            bool next = false;
            int[] dir1 = { 1, -1, 29, -29, 15, 14, -15, -14 };

            foreach (int i in dir1)
            {
                Los.Clear();
                celda_id = celda_1;
                Los.Add(celda_id);
                next = false;
                while (!next)
                {
                    celda_id += i;
                    Los.Add(celda_id);
                    if (get_Es_Borde2(celda_id) || get_Es_Borde1(celda_id) || celda_id <= 0 || celda_id >= 480)
                        next = true;
                    if (celda_id == celda_2)
                    {
                        return Los;
                    }
                }
            }
            return null;
        }

        public static bool get_Es_Borde1(int id)
        {
            int[] bordes = { 1, 30, 59, 88, 117, 146, 175, 204, 233, 262, 291, 320, 349, 378, 407, 436, 465, 15, 44, 73, 102, 131, 160, 189, 218, 247, 276, 305, 334, 363, 392, 421, 450, 479 };
            List<int> test = new List<int>();
            foreach (int i in bordes)
            {
                test.Add(i);
            }
            if (test.Contains(id))
                return true;
            else
                return false;
        }

        public static bool get_Es_Borde2(int id)
        {
            int[] bordes = { 16, 45, 74, 103, 132, 161, 190, 219, 248, 277, 306, 335, 364, 393, 422, 451, 29, 58, 87, 116, 145, 174, 203, 232, 261, 290, 319, 348, 377, 406, 435, 464 };
            List<int> test = new List<int>();
            foreach (int i in bordes)
            {
                test.Add(i);
            }
            if (test.Contains(id))
                return true;
            else
                return false;
        }

        #region Metodos
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

        public bool get_Esta_En_Linea(int celda_1, int celda_2)
        {
            bool X = get_Celda_X_Coordenadas(celda_1) == get_Celda_X_Coordenadas(celda_2);
            bool Y = get_Celda_Y_Coordenadas(celda_1) == get_Celda_Y_Coordenadas(celda_2);

            return X || Y;
        }

        public char getDireccionOpuesta(char c)
        {
            switch (c)
            {
                case 'a':
                    return 'e';
                case 'b':
                    return 'f';
                case 'c':
                    return 'g';
                case 'd':
                    return 'h';
                case 'e':
                    return 'a';
                case 'f':
                    return 'b';
                case 'g':
                    return 'c';
                case 'h':
                    return 'd';
            }
            return char.MaxValue;
        }

        public Celda get_Coordenadas(int x, int y) => celdas.FirstOrDefault(c => get_Celda_X_Coordenadas(c.id) == x && get_Celda_Y_Coordenadas(c.id) == y);
        #endregion

        #region Metodos de descompresion
        public void descomprimir_mapa()
        {
            celdas = new Celda[mapa_data.Length / 10];
            string valores_celda;

            for (int i = 0; i < mapa_data.Length; i += 10)
            {
                valores_celda = mapa_data.Substring(i, 10);
                celdas[i / 10] = descompimir_Celda(valores_celda, i / 10);
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
            celda.es_celda_interactiva = ((informacion_celda[7] & 2) >> 1) != 0;
            celda.objeto_interactivo_id = celda.es_celda_interactiva ? ((informacion_celda[0] & 2) << 12) + ((informacion_celda[7] & 1) << 12) + (informacion_celda[8] << 6) + informacion_celda[9] : -1;
            celda.layerGroundLevel = Convert.ToByte(informacion_celda[1] & 15);
            celda.layerGroundSlope = Convert.ToByte((informacion_celda[4] & 60) >> 2);

            return celda;
        }
        #endregion

        #region metodos monstruos
        public void agregar_Monstruo(Monstruo monstruo)
        {
            if (monstruos == null)
                monstruos = new Dictionary<int, Monstruo>();
            if (!monstruos.ContainsKey(monstruo.id))
                monstruos.Add(monstruo.id, monstruo);
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

        public Dictionary<int, Monstruo> get_Monstruos()
        {
            if (monstruos == null)
                return new Dictionary<int, Monstruo>();
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
