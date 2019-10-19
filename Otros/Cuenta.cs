using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game;
using Bot_Dofus_1._29._1.Otros.Grupos;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Scripts;
using Bot_Dofus_1._29._1.Utilities.Config;
using Bot_Dofus_1._29._1.Utilities.Logs;
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
    public class Account : IDisposable
    {
        public string nickname { get; set; } = string.Empty;
        public string welcomeKey { get; set; } = string.Empty;
        public string gameTicket { get; set; } = string.Empty;
        public Logger logger { get; private set; }
        public ClienteTcp connexion { get; set; }
        public Juego game { get; private set; }
        public ManejadorScript script { get; set; }
        public PeleaExtensiones fightExtension { get; set; }
        public AccountConfig accountConfig { get; private set; }
        private AccountStates accountState = AccountStates.DISCONNECTED;
        public bool canUseMount = false;

        public Grupo group { get; set; }
        public bool hasGroup => group != null;
        public bool isGroupLeader => !hasGroup || group.lider == this;

        private bool disposed;
        public event Action accountStateEvent;
        public event Action accountDisconnectEvent;

        public Account(AccountConfig _accountConfig)
        {
            accountConfig = _accountConfig;
            logger = new Logger();
            game = new Juego(this);
            fightExtension = new PeleaExtensiones(this);
            script = new ManejadorScript(this);
        }

        public void conectar()
        {
            connexion = new ClienteTcp(this);
            connexion.conexion_Servidor(IPAddress.Parse(GlobalConfig.loginIP), GlobalConfig.loginPort);
        }

        public void desconectar()
        {
            connexion?.Dispose();
            connexion = null;

            script.detener_Script();
            game.Clear();
            Estado_Cuenta = AccountStates.DISCONNECTED;
            accountDisconnectEvent?.Invoke();
        }

        public void cambiando_Al_Servidor_Juego(string ip, int puerto)
        {
            connexion.get_Desconectar_Socket();
            connexion.conexion_Servidor(IPAddress.Parse(ip), puerto);
        }

        public AccountStates Estado_Cuenta
        {
            get => accountState;
            set
            {
                accountState = value;
                accountStateEvent?.Invoke();
            }
        }

        public bool esta_ocupado() => Estado_Cuenta != AccountStates.CONNECTED_INACTIVE && Estado_Cuenta != AccountStates.REGENERATION;
        public bool esta_dialogando() => Estado_Cuenta == AccountStates.STORAGE || Estado_Cuenta == AccountStates.DIALOG || Estado_Cuenta == AccountStates.EXCHANGE || Estado_Cuenta == AccountStates.BUYING || Estado_Cuenta == AccountStates.SELLING;
        public bool esta_luchando() => Estado_Cuenta == AccountStates.FIGHTING;
        public bool esta_recolectando() => Estado_Cuenta == AccountStates.GATHERING;
        public bool esta_desplazando() => Estado_Cuenta == AccountStates.MOVING;

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Account() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    script.Dispose();
                    connexion?.Dispose();
                    game.Dispose();
                }
                Estado_Cuenta = AccountStates.DISCONNECTED;
                script = null;
                welcomeKey = null;
                connexion = null;
                logger = null;
                game = null;
                nickname = null;
                accountConfig = null;
                disposed = true;
            }
        }
        #endregion
    }
}
