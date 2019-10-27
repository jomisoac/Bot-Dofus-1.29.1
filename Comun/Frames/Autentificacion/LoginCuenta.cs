using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Server;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.LoginCuenta
{
    public class LoginCuenta : Frame
    {
        [PaqueteAtributo("HC")]
        public void get_Key_BienvenidaAsync(ClienteTcp cliente, string paquete)
        {
            Account cuenta = cliente.cuenta;

            cuenta.Estado_Cuenta = AccountStates.CONNECTED;
            cuenta.welcomeKey = paquete.Substring(2);

            cliente.enviar_Paquete("1.30");
            cliente.enviar_Paquete(cliente.cuenta.accountConfig.accountUsername + "\n" + Hash.Crypt_Password(cliente.cuenta.accountConfig.accountPassword, cliente.cuenta.welcomeKey));
            cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("Ad")]
        public void get_Apodo(ClienteTcp cliente, string paquete) => cliente.cuenta.nickname = paquete.Substring(2);

        [PaqueteAtributo("Af")]
        public void get_Fila_Espera_Login(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("File d'attente", "Position " + paquete[2] + "/" + paquete[4]);

        [PaqueteAtributo("AH")]
        public void get_Servidor_Estado(ClienteTcp cliente, string paquete)
        {
            Account cuenta = cliente.cuenta;
            string[] separado_servidores = paquete.Substring(2).Split('|');
            GameServer servidor = cuenta.game.servidor;
            bool primera_vez = true;

            foreach(string sv in separado_servidores)
            {
                string[] separador = sv.Split(';');

                int id = int.Parse(separador[0]);
                ServerStates estado = (ServerStates)byte.Parse(separador[1]);
                string nombre = cuenta.accountConfig.server;

                // Add Method to take name with Id

                if (id == cuenta.accountConfig.Get_Server_ID())
                {
                    servidor.actualizar_Datos(id, nombre, estado);
                    cuenta.logger.log_informacion("LOGIN", $"Le serveur {nombre} est {cuenta.game.servidor.GetState(estado)}");

                    if (estado != ServerStates.ONLINE)
                        primera_vez = false;
                }
            }

            if(!primera_vez  && servidor.estado == ServerStates.ONLINE)
                cliente.enviar_Paquete("Ax");
        }

        [PaqueteAtributo("AQ")]
        public void get_Pregunta_Secreta(ClienteTcp cliente, string paquete)
        {
            if (cliente.cuenta.game.servidor.estado == ServerStates.ONLINE)
                cliente.enviar_Paquete("Ax", true);
        }

        [PaqueteAtributo("AxK")]
        public void get_Servidores_Lista(ClienteTcp cliente, string paquete)
        {
            Account cuenta = cliente.cuenta;
            string[] loc5 = paquete.Substring(3).Split('|');
            int contador = 1;
            bool seleccionado = false;

            while (contador < loc5.Length && !seleccionado)
            {
                string[] _loc10_ = loc5[contador].Split(',');
                int servidor_id = int.Parse(_loc10_[0]);

                if (servidor_id == cuenta.game.servidor.id)
                {
                    if(cuenta.game.servidor.estado == ServerStates.ONLINE)
                    {
                        seleccionado = true;
                        cuenta.game.personaje.evento_Servidor_Seleccionado();
                    }
                    else
                        cuenta.logger.log_Error("LOGIN", "Serveur non accessible lorsque celui-ci se reconnectera");
                }
                contador++;
            }

            if(seleccionado)
                cliente.enviar_Paquete($"AX{cuenta.game.servidor.id}", true);
        }

        [PaqueteAtributo("AXK")]
        public void get_Seleccion_Servidor(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.gameTicket = paquete.Substring(14);
            cliente.cuenta.cambiando_Al_Servidor_Juego(Hash.Decrypt_IP(paquete.Substring(3, 8)), Hash.Decrypt_Port(paquete.Substring(11, 3).ToCharArray()));
        }
    }
}
