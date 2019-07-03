using System;
using System.Windows.Forms;
using System.Reflection;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Oficios;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Oficios : UserControl
    {
        private Cuenta cuenta;

        public UI_Oficios()
        {
            InitializeComponent();
            set_DoubleBuffered(dataGridView_oficios);
            set_DoubleBuffered(dataGridView_skills);
        }

        public void set_Cuenta(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            cuenta.juego.personaje.oficios_actualizados += personaje_Oficios_Actualizados;
        }

        private void personaje_Oficios_Actualizados()
        {
            BeginInvoke((Action)(() =>
            {
                dataGridView_oficios.Rows.Clear();
                foreach (Oficio oficio in cuenta.juego.personaje.oficios)
                    dataGridView_oficios.Rows.Add(new object[] { oficio.id, oficio.nombre, oficio.nivel, oficio.experiencia_actual + "/" + oficio.experiencia_siguiente_nivel, oficio.get_Experiencia_Porcentaje + "%" });

                dataGridView_skills.Rows.Clear();
                foreach (SkillsOficio skill in cuenta.juego.personaje.get_Skills_Disponibles())
                    dataGridView_skills.Rows.Add(new object[] { skill.id, skill.interactivo_modelo.nombre, skill.cantidad_minima, skill.cantidad_maxima, skill.es_craft ? skill.tiempo + "%" : skill.tiempo.ToString() });
            }));
        }

        public static void set_DoubleBuffered(Control control) => typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, control, new object[] { true });
    }
}
