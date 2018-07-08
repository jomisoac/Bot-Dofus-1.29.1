using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
       
            Task.Factory.StartNew(() =>
            {
                GlobalConfiguracion.cargar_Todas_Cuentas();
            });

            Application.Run(new FormPrincipal());
        }
    }
}
