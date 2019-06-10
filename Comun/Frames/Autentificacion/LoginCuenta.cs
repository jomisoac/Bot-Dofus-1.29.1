using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

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
            Cuenta cuenta = cliente.cuenta;

            cuenta.Estado_Cuenta = EstadoCuenta.CONECTANDO;
            cliente.Estado_Socket = EstadoSocket.LOGIN;
            cuenta.key_bienvenida = paquete.Substring(2);

            cliente.enviar_Paquete("1.29.1");
            cliente.enviar_Paquete(cliente.cuenta.cuenta_configuracion.nombre_cuenta + "\n" + Hash.encriptar_Password(cliente.cuenta.cuenta_configuracion.password, cliente.cuenta.key_bienvenida));
            cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("Ad")]
        public void get_Apodo(ClienteTcp cliente, string paquete) => cliente.cuenta.apodo = paquete.Substring(2);

        [PaqueteAtributo("Af")]
        public void get_Fila_Espera_Login(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("FILA DE ESPERA", "Posición " + paquete[2] + "/" + paquete[4]);

        [PaqueteAtributo("AH")]
        public void get_Servidor_Estado(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] _loc5_ = paquete.Substring(2).Split('|');
            int _loc6_ = 0;
            bool accesible = true;

            while (_loc6_ < _loc5_.Length && accesible)
            {
                string[] _loc7_ = _loc5_[_loc6_].ToString().Split(';');
                int id = int.Parse(_loc7_[0]);
                byte estado = byte.Parse(_loc7_[1]);
                byte poblacion = byte.Parse(_loc7_[2]);
                bool registro = _loc7_[3] == "1";

                if (id == cuenta.servidor_id)
                {
                    cliente.cuenta.logger.log_informacion("LOGIN", "El servidor " + (id == 601 ? "Eratz" : "Henual") + " esta " + (EstadosServidor)estado);

                    if (estado != 1)
                    {
                        cliente.cuenta.logger.log_Error("LOGIN", "Servidor no accesible cuando este accesible se re-conectara");
                        accesible = false;
                    }
                }

                _loc6_++;
            }

            if (accesible)
                cliente.enviar_Paquete("Ax");
        }

        [PaqueteAtributo("AxK")]
        public void get_Servidores_Lista(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] loc5 = paquete.Substring(3).Split('|');
            int contador = 1;
            bool seleccionado = false;

            while (contador < loc5.Length && !seleccionado)
            {
                string[] _loc10_ = loc5[contador].Split(',');
                int servidor_id = int.Parse(_loc10_[0]);

                if (cuenta.servidor_id == servidor_id)
                {
                    cliente.enviar_Paquete("AX" + cuenta.servidor_id);
                    seleccionado = true;
                }

                contador++;
            }
        }

        [PaqueteAtributo("AXK")]
        public void get_Seleccion_Servidor(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.tiquet_game = paquete.Substring(14);
            cliente.cuenta.cambiando_Al_Servidor_Juego(Hash.desencriptar_Ip(paquete.Substring(3, 8)), Hash.desencriptar_Puerto(paquete.Substring(11, 3).ToCharArray()));
        }
    }
}
