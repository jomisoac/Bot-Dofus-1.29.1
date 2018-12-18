using System.Windows.Forms;
using Bot_Dofus_1._29._1.Otros;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Pelea : UserControl
    {
        private Cuenta cuenta;

        public UI_Pelea(Cuenta _cuenta)
        {
            InitializeComponent();
            cuenta = _cuenta;
        }
    }
}
