using System;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Npcs
{
    public class Npcs : Entidad
    {
        public int id { get; set; }
        public int npc_id { get; private set; }
        public int celda_id { get; set; }
        private bool disposed;

        public Npcs(int _id, int _npc_id, int _celda_id)
        {
            id = _id;
            npc_id = _npc_id;
            celda_id = _celda_id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
    }
}
