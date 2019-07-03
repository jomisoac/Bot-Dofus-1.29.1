using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Oficios
{
    public class Oficio
    {
        public int id { get; private set; }
        public byte nivel { get; set; }
        public string nombre { get; private set; }
        public uint experiencia_base { get; private set; }
        public uint experiencia_actual { get; private set; }
        public uint experiencia_siguiente_nivel { get; private set; }
        public List<SkillsOficio> skills { get; private set; }
        
        public Oficio(int _id)
        {
            id = _id;
            nombre = get_Nombre_Oficio(id);
            skills = new List<SkillsOficio>();
        }

        private string get_Nombre_Oficio(int id_oficio) => XElement.Parse(Properties.Resources.oficios).Elements("OFICIO").Where(e => int.Parse(e.Element("id").Value) == id_oficio).Elements("nombre").Select(e => e.Value).FirstOrDefault();
        public double get_Experiencia_Porcentaje => experiencia_actual == 0 ? 0 : Math.Round((double)(experiencia_actual - experiencia_base) / (experiencia_siguiente_nivel - experiencia_base) * 100, 2);

        public void set_Actualizar_Oficio(byte _nivel, uint _experiencia_base, uint _experiencia_actual, uint _experiencia_siguiente_nivel)
        {
            nivel = _nivel;
            experiencia_base = _experiencia_base;
            experiencia_actual = _experiencia_actual;
            experiencia_siguiente_nivel = _experiencia_siguiente_nivel;
        }
    }
}
