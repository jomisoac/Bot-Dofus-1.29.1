using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
       
            Task.Run(() =>
            {
                GlobalConf.cargar_Todas_Cuentas();
                XElement.Parse(Properties.Resources.interactivos).Descendants("SKILL").ToList().ForEach(i => new ObjetoInteractivoModelo(i.Element("nombre").Value, i.Element("gfx").Value, bool.Parse(i.Element("caminable").Value), i.Element("habilidades").Value, bool.Parse(i.Element("recolectable").Value)));
            });

            paquete_recibido = new PaqueteRecibido();
            paquete_recibido.Inicializar();
            Application.Run(new Principal());
        }
    }
}
