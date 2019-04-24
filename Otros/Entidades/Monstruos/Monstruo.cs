using System;
using System.Collections.Generic;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Entidades.Monstruos
{
    public class Monstruo : Entidad
    {
        public int id { get; set; } = 0;
        public int template_id { get; set; } = 0;
        public int celda_id { get; set; }
        public int nivel { get; set; }

        public List<Monstruo> moobs_dentro_grupo { get; set; }
        public Monstruo lider_grupo { get; set; }
        bool disposed;

        public int get_Total_Monstruos => moobs_dentro_grupo.Count + 1;
        public int get_Total_Nivel_Grupo => lider_grupo.nivel + moobs_dentro_grupo.Sum(f => f.nivel);

        public Monstruo(int _id, int _template, int _celda_id, int _nivel)
        {
            id = _id;
            template_id = _template;
            celda_id = _celda_id;
            moobs_dentro_grupo = new List<Monstruo>();
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

        ~Monstruo() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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