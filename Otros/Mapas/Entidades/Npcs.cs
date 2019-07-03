using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Otros.Mapas.Entidades
{
    public class Npcs : Entidad
    {
        public int id { get; set; }
        public int npc_modelo_id { get; private set; }
        public Celda celda { get; set; }

        public short pregunta { get; set; }
        public List<short> respuestas { get; set; }
        private bool disposed;

        public Npcs(int _id, int _npc_modelo_id, Celda _celda)
        {
            id = _id;
            npc_modelo_id = _npc_modelo_id;
            celda = _celda;
        }

        #region Zona Dispose
        ~Npcs() => Dispose(false);
        public void Dispose() => Dispose(true);

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                respuestas?.Clear();
                respuestas = null;
                celda = null;
                disposed = true;
            }
        }
        #endregion
    }
}
