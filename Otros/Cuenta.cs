using System;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Scripts;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Game;
using Bot_Dofus_1._29._1.Protocolo.Login;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Logs;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
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
        public ClienteProtocolo conexion = null;
        public Personaje personaje { get; set; }
        public ManejadorScript script { get; set; }
        public PeleaExtensiones pelea_extension { get; set; }
        public CuentaConf cuenta_configuracion { get; private set; }
        private EstadoCuenta estado_cuenta = EstadoCuenta.DESCONECTADO;
        private EstadoSocket fase_socket = EstadoSocket.NINGUNO;
        private bool disposed;

        public event Action evento_estado_cuenta;
        public event Action<ClienteProtocolo> evento_fase_socket;

        public Cuenta(CuentaConf _cuenta_configuracion)
        {
            cuenta_configuracion = _cuenta_configuracion;
            servidor_id = cuenta_configuracion.get_Servidor_Id();
            logger = new Logger();
            pelea_extension = new PeleaExtensiones(this);
            conexion = new Login(GlobalConf.ip_conexion, 443, this);
        }

        public string get_Nombre_Servidor() => servidor_id == 601 ? "Eratz" : "Henual";

        public void cambiando_Al_Servidor_Juego(string ip, int puerto)
        {
            if (fase_socket != EstadoSocket.NINGUNO)
            {
                conexion = new Game(ip, puerto, this);
                Estado_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
            }
        }

        public EstadoCuenta Estado_Cuenta
        {
            get => estado_cuenta;
            set
            {
                Console.WriteLine(value);
                estado_cuenta = value;
                evento_estado_cuenta?.Invoke();
            }
        }

        public EstadoSocket Estado_Socket
        {
            get => fase_socket;
            internal set
            {
                EstadoSocket antiguo_valor = fase_socket;
                fase_socket = value;

                if (antiguo_valor != fase_socket)
                {
                    evento_fase_socket?.Invoke(conexion);
                }
            }
        }

        public bool esta_ocupado => Estado_Cuenta != EstadoCuenta.CONECTADO_INACTIVO && Estado_Cuenta != EstadoCuenta.REGENERANDO_VIDA;
        public bool esta_dialogando() => Estado_Cuenta == EstadoCuenta.ALMACENAMIENTO || Estado_Cuenta == EstadoCuenta.HABLANDO || Estado_Cuenta == EstadoCuenta.INTERCAMBIO || Estado_Cuenta == EstadoCuenta.COMPRANDO || Estado_Cuenta == EstadoCuenta.VENDIENDO;
        public bool esta_luchando() => Estado_Cuenta == EstadoCuenta.LUCHANDO;
        public bool esta_recolectando() => Estado_Cuenta == EstadoCuenta.RECOLECTANDO;

        ~Cuenta() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if (disposing)
                {
                    if (script != null)
                        script.Dispose();
                    if (conexion != null)
                        conexion.Dispose();
                    if (personaje != null)
                        personaje.Dispose();
                }
                key_bienvenida = null;
                conexion = null;
                logger = null;
                personaje = null;
                apodo = null;
                cuenta_configuracion = null;
                disposed = true;
                Estado_Cuenta = EstadoCuenta.DESCONECTADO;
                Estado_Socket = EstadoSocket.NINGUNO;
            }
        }
    }
}
