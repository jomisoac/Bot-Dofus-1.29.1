using System;
using System.Text;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Personaje : UserControl
    {
        private Cuenta cuenta;

        public UI_Personaje(Cuenta _cuenta)
        {
            InitializeComponent();

            cuenta = _cuenta;
            cuenta.personaje.personaje_seleccionado += personaje_Seleccionado_Servidor_Juego;
        }

        private void personaje_Seleccionado_Servidor_Juego()
        {
            BeginInvoke((Action)(() =>
            {
                StringBuilder URL = new StringBuilder().Append("http://staticns.ankama.com/dofus/renderer/look/");
                URL.Append(Hash.encriptar_hexadecimal("{1|" + cuenta.personaje.gfxID + ",2164,3419,3429,3576,3488|1=8537887,2=0,3=0,4=13738269,5=13738269|145}")).Append("/full/1/256_450-0.png");
                imagen_personaje.LoadAsync(URL.ToString());

                label_nombre_personaje.Text = cuenta.personaje.nombre_personaje;
                label_nivel_personaje.Text = $"Nivel {cuenta.personaje.nivel}";
            }));
        }
    }
}
