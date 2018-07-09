using System.Windows.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class Cuenta : UserControl
    {
        public CuentaConfiguracion configuracion_cuenta;

        public Cuenta(CuentaConfiguracion _configuracion_cuenta)
        {
            InitializeComponent();
            configuracion_cuenta = _configuracion_cuenta;
        }
    }
}
