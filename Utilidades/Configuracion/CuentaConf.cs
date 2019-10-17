using Bot_Dofus_1._29._1.Controles.ControlMapa;
using System.IO;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    public class CuentaConf
    {
        public string nombre_cuenta { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string servidor { get; set; } = string.Empty;
        public string nombre_personaje { get; set; } = string.Empty;

        public CuentaConf(string _nombre_cuenta, string _password, string _servidor, string _nombre_personaje)
        {
            nombre_cuenta = _nombre_cuenta;
            password = _password;
            servidor = _servidor;
            nombre_personaje = _nombre_personaje;
        }

        public void guardar_Cuenta(BinaryWriter bw)
        {
            bw.Write(nombre_cuenta);
            bw.Write(password);
            bw.Write(servidor);
            bw.Write(nombre_personaje);
        }

        public static CuentaConf cargar_Una_Cuenta(BinaryReader br)
        {
            try
            {
                return new CuentaConf(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString());
            }
            catch
            {
                return null;
            }
        }

        public int get_Servidor_Id()
        {
            switch (servidor)
            {
                case "Eratz":
                    return 601;
                case "Henual":
                    return 602;
                case "Nabur":
                    return 603;
                case "Arty":
                    return 604;
                case "Algathe":
                    return 605;
                case "Hogmeiser":
                    return 606;
                case "Droupik":
                    return 607;
                case "Ayuto":
                    return 608;
                case "Bilby":
                    return 609;
                case "Clustus":
                    return 610;
                default:
                    return 601;
                    break;
            }
        }
    }
}
