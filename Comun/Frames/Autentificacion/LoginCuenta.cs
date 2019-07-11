using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Servidor;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
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
            Cuenta cuenta = cliente.cuenta;

            cuenta.Estado_Cuenta = EstadoCuenta.CONECTANDO;
            cuenta.key_bienvenida = paquete.Substring(2);

            cliente.enviar_Paquete("1.29.1");
            cliente.enviar_Paquete(cliente.cuenta.configuracion.nombre_cuenta + "\n" + Hash.encriptar_Password(cliente.cuenta.configuracion.password, cliente.cuenta.key_bienvenida));
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
            string[] separado_servidores = paquete.Substring(2).Split('|');
            ServidorJuego servidor = cuenta.juego.servidor;
            bool primera_vez = true;

            foreach(string sv in separado_servidores)
            {
                string[] separador = sv.Split(';');

                int id = int.Parse(separador[0]);
                EstadosServidor estado = (EstadosServidor)byte.Parse(separador[1]);
                string nombre = id == 601 ? "Eratz" : "Henual";

                if (id == cuenta.configuracion.get_Servidor_Id())
                {
                    servidor.actualizar_Datos(id, nombre, estado);
                    cuenta.logger.log_informacion("LOGIN", $"El servidor {nombre} esta {estado}");

                    if (estado != EstadosServidor.CONECTADO)
                        primera_vez = false;
                }
            }

            if(!primera_vez  && servidor.estado == EstadosServidor.CONECTADO)
                cliente.enviar_Paquete("Ax");
        }

        [PaqueteAtributo("AQ")]
        public void get_Pregunta_Secreta(ClienteTcp cliente, string paquete)
        {
            if (cliente.cuenta.juego.servidor.estado == EstadosServidor.CONECTADO)
                cliente.enviar_Paquete("Ax", true);
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

                if (servidor_id == cuenta.juego.servidor.id)
                {
                    if(cuenta.juego.servidor.estado == EstadosServidor.CONECTADO)
                    {
                        seleccionado = true;
                        cuenta.juego.personaje.evento_Servidor_Seleccionado();
                    }
                    else
                        cuenta.logger.log_Error("LOGIN", "Servidor no accesible cuando este accesible se re-conectara");
                }
                contador++;
            }

            if(seleccionado)
                cliente.enviar_Paquete($"AX{cuenta.juego.servidor.id}", true);
        }

        [PaqueteAtributo("AXK")]
        public void get_Seleccion_Servidor(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.tiquet_game = paquete.Substring(14);
            cliente.cuenta.cambiando_Al_Servidor_Juego(Hash.desencriptar_Ip(paquete.Substring(3, 8)), Hash.desencriptar_Puerto(paquete.Substring(11, 3).ToCharArray()));
        }
    }
}
