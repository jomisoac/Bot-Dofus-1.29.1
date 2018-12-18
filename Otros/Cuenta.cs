using System;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros.Personajes;
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
        public string usuario { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string apodo { get; set; } = string.Empty;
        public string key_bienvenida { get; set; } = string.Empty;
        public string tiquet_game { get; set; } = string.Empty;
        public int servidor_id { get; set; } = 0;
        public Logger logger { get; private set; }
        public ClienteProtocolo conexion = null;
        public Personaje personaje { get; set; }
        public ManejadorScript script { get; set; }
        private EstadoCuenta estado_cuenta = EstadoCuenta.DESCONECTADO;
        private EstadoSocket fase_socket = EstadoSocket.NINGUNO;

        public event Action evento_estado_cuenta;
        public event Action<ClienteProtocolo> evento_fase_socket;
        
        public Cuenta(string _usuario, string _password, int _servidor_id)
        {
            usuario = _usuario;
            password = _password;
            servidor_id = _servidor_id;
            logger = new Logger();
            conexion = new Login(GlobalConf.ip_conexion, 443, this);
        }

        public string get_Nombre_Servidor() => servidor_id == 601 ? "Eratz" : "Henual";

        public void cambiando_Al_Servidor_Juego(string ip, int puerto)
        {
            if (fase_socket != EstadoSocket.NINGUNO)
            {
                conexion = new Game(ip, puerto, this);
                Fase_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
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

        public EstadoSocket Fase_Socket
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

        ~Cuenta()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if(personaje != null)
                        personaje.Dispose();
                    if(script != null)
                        script.Dispose();
                    if(conexion != null)
                        conexion.Dispose();
                }
                usuario = null;
                password = null;
                key_bienvenida = null;
                conexion = null;
                logger = null;
                personaje = null;
                apodo = null;
                Estado_Cuenta = EstadoCuenta.DESCONECTADO;
                Fase_Socket = EstadoSocket.NINGUNO;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
