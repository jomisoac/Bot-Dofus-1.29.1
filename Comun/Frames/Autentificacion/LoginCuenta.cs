using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Protocolo;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Login.Paquetes;
using Bot_Dofus_1._29._1.Utilidades;
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
        public void get_Key_BienvenidaAsync(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTANDO;
            cliente.cuenta.Estado_Socket = EstadoSocket.LOGIN;
            cliente.cuenta.key_bienvenida = paquete.Substring(2);

            cliente.enviar_Paquete(Constantes.VERSION + "." + Constantes.SUBVERSION + "." + Constantes.SUBSUBVERSION);
            cliente.enviar_Paquete(cliente.cuenta.cuenta_configuracion.nombre_cuenta + "\n" + Hash.encriptar_Password(cliente.cuenta.cuenta_configuracion.password, cliente.cuenta.key_bienvenida));
            cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("Ad")]
        public void get_Apodo(ClienteAbstracto cliente, string paquete) => cliente.cuenta.apodo = paquete.Substring(2);

        [PaqueteAtributo("Af")]
        public void get_Fila_Espera_Login(ClienteAbstracto cliente, string paquete) => cliente.cuenta.logger.log_informacion("FILA DE ESPERA", "Posición " + paquete[2] + "/" + paquete[4]);

        [PaqueteAtributo("AH")]
        public void get_Servidor_Estado(ClienteAbstracto cliente, string paquete)
        {
            HostsMensaje servidor = new HostsMensaje(paquete.Substring(3), cliente.cuenta.servidor_id);
            cliente.cuenta.logger.log_informacion("Login", "El servidor " + cliente.cuenta.get_Nombre_Servidor() + " esta " + (HostsMensaje.EstadosServidor)servidor.estado);

            if((HostsMensaje.EstadosServidor)servidor.estado == HostsMensaje.EstadosServidor.CONECTADO)
                cliente.enviar_Paquete("Ax");
            else
            {
                cliente.cuenta.logger.log_Error("Login", "Desconectando del servidor, para evitar anti-bot");
                cliente.get_Desconectar_Socket();
            } 
        }

        [PaqueteAtributo("AxK")]
        public void get_Servidores_Lista(ClienteAbstracto cliente, string paquete) => cliente.enviar_Paquete(new ListaServidoresMensaje(paquete.Substring(3), cliente.cuenta.servidor_id).get_Mensaje());

        [PaqueteAtributo("AXK")]
        public void get_Seleccion_Servidor(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.tiquet_game = paquete.Substring(14);
            cliente.cuenta.cambiando_Al_Servidor_Juego(Hash.desencriptar_Ip(paquete.Substring(3, 8)), Hash.desencriptar_Puerto(paquete.Substring(11, 3).ToCharArray()));

            cliente.cuenta.Estado_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
        }
    }
}
