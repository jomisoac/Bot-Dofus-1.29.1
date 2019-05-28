using Bot_Dofus_1._29._1.Controles.ControlMapa;
using System.IO;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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
        public int id_personaje { get; set; } = 0;

        public CuentaConf(string _nombre_cuenta, string _password, string _servidor, int _id_personaje)
        {
            nombre_cuenta = _nombre_cuenta;
            password = _password;
            servidor = _servidor;
            id_personaje = _id_personaje;
        }

        public void guardar_Cuenta(BinaryWriter bw)
        {
            bw.Write(nombre_cuenta);
            bw.Write(password);
            bw.Write(servidor);
            bw.Write(id_personaje);
        }

        public static CuentaConf cargar_Una_Cuenta(BinaryReader br)
        {
            try
            {
                return new CuentaConf(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadInt32());
            }
            catch
            {
                return null;
            }
        }

        public int get_Servidor_Id() => servidor.Equals("Eratz") ? 601 : 602;
    }
}
