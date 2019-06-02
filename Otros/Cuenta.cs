using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Scripts;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Logs;
using System;
using System.Net;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros
{
    public class Cuenta : IDisposable
    {
        public string apodo { get; set; } = string.Empty;
        public string key_bienvenida { get; set; } = string.Empty;
        public string tiquet_game { get; set; } = string.Empty;
        public int servidor_id { get; set; } = 0;
        public Logger logger { get; private set; }
        public ClienteTcp conexion { get; set; }
        public Juego juego { get; private set; }
        public ManejadorScript script { get; set; }
        public PeleaExtensiones pelea_extension { get; set; }
        public CuentaConf cuenta_configuracion { get; private set; }
        public Pelea pelea;
        private EstadoCuenta estado_cuenta = EstadoCuenta.DESCONECTADO;
        public bool puede_utilizar_dragopavo { get; set; } = false;
        private bool disposed;

        public event Action evento_estado_cuenta;

        public Cuenta(CuentaConf _cuenta_configuracion)
        {
            cuenta_configuracion = _cuenta_configuracion;
            servidor_id = cuenta_configuracion.get_Servidor_Id();
            logger = new Logger();
            pelea = new Pelea(this);
            juego = new Juego(this);
            pelea_extension = new PeleaExtensiones(this);
            script = new ManejadorScript(this);
            conexion = new ClienteTcp(this);
        }

        public string get_Nombre_Servidor() => servidor_id == 601 ? "Eratz" : "Henual";

        public void cambiando_Al_Servidor_Juego(string ip, int puerto)
        {
            if (conexion.Estado_Socket != EstadoSocket.NINGUNO)
            {
                conexion.Estado_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
                conexion.get_Desconectar_Socket();
                conexion.conexion_Servidor(IPAddress.Parse(ip), puerto);
            }
        }

        public EstadoCuenta Estado_Cuenta
        {
            get => estado_cuenta;
            set
            {
                estado_cuenta = value;
                evento_estado_cuenta?.Invoke();
            }
        }

        public bool esta_ocupado => Estado_Cuenta != EstadoCuenta.CONECTADO_INACTIVO && Estado_Cuenta != EstadoCuenta.REGENERANDO_VIDA;
        public bool esta_dialogando() => Estado_Cuenta == EstadoCuenta.ALMACENAMIENTO || Estado_Cuenta == EstadoCuenta.HABLANDO || Estado_Cuenta == EstadoCuenta.INTERCAMBIO || Estado_Cuenta == EstadoCuenta.COMPRANDO || Estado_Cuenta == EstadoCuenta.VENDIENDO;
        public bool esta_luchando() => Estado_Cuenta == EstadoCuenta.LUCHANDO;
        public bool esta_recolectando() => Estado_Cuenta == EstadoCuenta.RECOLECTANDO;
        public bool esta_desplazando() => Estado_Cuenta == EstadoCuenta.MOVIMIENTO;

        public void Dispose() => Dispose(true);
        ~Cuenta() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    script?.Dispose();
                    conexion?.Dispose();
                    juego?.Dispose();
                    pelea?.Dispose();
                }
                Estado_Cuenta = EstadoCuenta.DESCONECTADO;

                key_bienvenida = null;
                conexion = null;
                logger = null;
                juego = null;
                apodo = null;
                cuenta_configuracion = null;
                pelea = null;
                disposed = true;
            }
        }
    }
}
