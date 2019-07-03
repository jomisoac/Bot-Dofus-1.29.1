using System.Collections.Generic;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Mapas.Entidades
{
    public class Monstruos : Entidad
    {
        public int id { get; set; } = 0;
        public int template_id { get; set; } = 0;
        public Celda celda { get; set; }
        public int nivel { get; set; }

        public List<Monstruos> moobs_dentro_grupo { get; set; }
        public Monstruos lider_grupo { get; set; }
        bool disposed;

        public int get_Total_Monstruos => moobs_dentro_grupo.Count + 1;
        public int get_Total_Nivel_Grupo => lider_grupo.nivel + moobs_dentro_grupo.Sum(f => f.nivel);

        public Monstruos(int _id, int _template, Celda _celda, int _nivel)
        {
            id = _id;
            template_id = _template;
            celda = _celda;
            moobs_dentro_grupo = new List<Monstruos>();
            nivel = _nivel;
        }

        public bool get_Contiene_Monstruo(int id)
        {
            if (lider_grupo.template_id == id)
                return true;

            for (int i = 0; i < moobs_dentro_grupo.Count; i++)
            {
                if (moobs_dentro_grupo[i].template_id == id)
                    return true;
            }
            return false;
        }

        public void Dispose() => Dispose(true);
        ~Monstruos() => Dispose(false);
        
        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                moobs_dentro_grupo.Clear();
                moobs_dentro_grupo = null;
                disposed = true;
            }
        }
    }
}