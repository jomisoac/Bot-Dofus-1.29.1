/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Entidades
{
    public class Personajes : Entidad
    {
        public int id { get; set; } = 0;
        public string nombre { get; set; }
        public byte sexo { get; set; } = 0;
        public Celda celda { get; set; }
        private bool disposed;

        public Personajes(int _id, string _nombre_personaje, byte _sexo, Celda _celda)
        {
            id = _id;
            nombre = _nombre_personaje;
            sexo = _sexo;
            celda = _celda;
        }

        #region Zona Dispose
        ~Personajes() => Dispose(false);
        public void Dispose() => Dispose(true);

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                celda = null;
                disposed = true;
            }
        }
        #endregion
    }
}
