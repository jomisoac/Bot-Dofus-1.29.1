using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.Otros.Game.Character.Spells;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using Bot_Dofus_1._29._1.Utilities.Config;
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
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Task.Run(() =>
            {
                GlobalConfig.Load_All_Accounts();
                LuaScriptManager.inicializar_Funciones();
                XElement.Parse(Properties.Resources.interactivos).Descendants("SKILL").ToList().ForEach(i => new ObjetoInteractivoModelo(i.Element("nombre").Value, i.Element("gfx").Value, bool.Parse(i.Element("caminable").Value), i.Element("habilidades").Value, bool.Parse(i.Element("recolectable").Value)));
                PaqueteRecibido.Inicializar();
            }).ContinueWith(t =>
            {
                XElement.Parse(Properties.Resources.hechizos).Descendants("HECHIZO").ToList().ForEach(mapa =>
                {
                    Spell hechizo = new Spell(short.Parse(mapa.Attribute("ID").Value), mapa.Element("NOMBRE").Value);
                    
                    mapa.Descendants("NIVEL").ToList().ForEach(stats =>
                    {
                        SpellStats hechizo_stats = new SpellStats();

                        hechizo_stats.coste_pa = byte.Parse(stats.Attribute("COSTE_PA").Value);
                        hechizo_stats.alcanze_minimo = byte.Parse(stats.Attribute("RANGO_MINIMO").Value);
                        hechizo_stats.alcanze_maximo = byte.Parse(stats.Attribute("RANGO_MAXIMO").Value);

                        hechizo_stats.es_lanzado_linea = bool.Parse(stats.Attribute("LANZ_EN_LINEA").Value);
                        hechizo_stats.es_lanzado_con_vision = bool.Parse(stats.Attribute("NECESITA_VISION").Value);
                        hechizo_stats.es_celda_vacia = bool.Parse(stats.Attribute("NECESITA_CELDA_LIBRE").Value);
                        hechizo_stats.es_alcanze_modificable = bool.Parse(stats.Attribute("RANGO_MODIFICABLE").Value);

                        hechizo_stats.lanzamientos_por_turno = byte.Parse(stats.Attribute("MAX_LANZ_POR_TURNO").Value);
                        hechizo_stats.lanzamientos_por_objetivo = byte.Parse(stats.Attribute("MAX_LANZ_POR_OBJETIVO").Value);
                        hechizo_stats.intervalo = byte.Parse(stats.Attribute("COOLDOWN").Value);

                        stats.Descendants("EFECTO").ToList().ForEach(efecto => hechizo_stats.agregar_efecto(new SpellEffect(int.Parse(efecto.Attribute("TIPO").Value), Zones.Parse(efecto.Attribute("ZONA").Value)), bool.Parse(efecto.Attribute("ES_CRITICO").Value)));
                        hechizo.get_Agregar_Hechizo_Stats(byte.Parse(stats.Attribute("NIVEL").Value), hechizo_stats);
                    });
                });
            }).Wait();

            Application.Run(new Principal());
        }
    }
}