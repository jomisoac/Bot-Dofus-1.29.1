using System.Drawing;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.TabControl
{
    public class Pagina
    {
        public Cabezera cabezera { get; private set; }
        public Panel contenido { get; private set; }

        public Pagina(string nuevo_titulo, int anchura_cabezera)
        {
            cabezera = new Cabezera()
            {
                propiedad_Cuenta = nuevo_titulo,
                Size = new Size(anchura_cabezera, 40),
                Margin = new Padding(2, 0, 2, 10)
            };

            contenido = new Panel()
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
        }
    }
}
