using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1
{
    static class Program
    {
        public static PaqueteRecibido paquete_recibido;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
       
            Task.Run(() => GlobalConf.cargar_Todas_Cuentas());
            paquete_recibido = new PaqueteRecibido();
            paquete_recibido.Inicializar();

            Application.Run(new Principal());
        }
    }
}
