using System;
using System.Text;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Login.Paquetes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2018 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Protocolo.Login
{
    public class Login : ClienteProtocolo
    {
        private Cuenta cuenta = null;

        public Login(string ip, int puerto, Cuenta _cuenta) : base(ip, puerto)
        {
            cuenta = _cuenta;
            paquete_recibido += analisis_Paquete;
        }

        public void analisis_Paquete(string paquete)
        {
            char accion = paquete[1];
            bool tiene_error = false;

            if (paquete.Length >= 3)
            {
                tiene_error = (paquete[2] == 'E');
            }

            switch (paquete[0])
            {
                case 'H':
                    switch (accion)
                    {
                        case 'C':
                            cuenta.Estado_Cuenta = EstadoCuenta.CONECTANDO;
                            cuenta.Estado_Socket = EstadoSocket.LOGIN;
                            cuenta.key_bienvenida = paquete.Substring(2);
                            enviar_Paquete(new VersionMensaje().get_Mensaje());
                            enviar_Paquete(new LoginMensaje(cuenta.cuenta_configuracion.nombre_cuenta, cuenta.cuenta_configuracion.password, cuenta.key_bienvenida).get_Mensaje());
                            enviar_Paquete(new FilaPosicionMensaje().get_Mensaje());
                        break;

                        default:
                            Console.WriteLine("Paquete desconocido: " + paquete);
                        break;
                    }
                break;

                case 'A':
                    switch (accion)
                    {
                        case 'd'://paquete de apodo
                            cuenta.apodo = paquete.Substring(2);
                        break;

                        case 'q'://paquete fila de espera
                        case 'f'://paquete fila de espera
                            cuenta.logger.log_informacion("FILA DE ESPERA", "Posición " + paquete[2] + "/" + paquete[4]);
                        break;

                        case 'Q'://paquete pregunta secreta (no tiene ningún valor)
                        break;

                        case 'H':
                            HostsMensaje servidor = new HostsMensaje(paquete.Substring(3), cuenta.servidor_id);
                            enviar_Paquete(servidor.get_Mensaje());
                            cuenta.logger.log_informacion("Login", "El servidor " + cuenta.get_Nombre_Servidor() + " esta " + (HostsMensaje.EstadosServidor)servidor.estado);
                            break;

                        case 'x':
                            enviar_Paquete(new ListaServidoresMensaje(paquete.Substring(3), cuenta.servidor_id).get_Mensaje());
                            break;

                        case 'X':
                            conectar_Game_Server(tiene_error, paquete.Substring(3));
                            cuenta.Estado_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
                        break;

                        case 'l':
                            switch (paquete[2])
                            {
                                case 'E':
                                    switch (paquete[3])
                                    {
                                        case 'f':
                                            cuenta.logger.log_Error("Login", "Conexión rechazada. Nombre de cuenta o contraseña incorrectos.");
                                            cerrar_Socket();
                                        break;

                                        case 'v':
                                            cuenta.logger.log_Error("Login", "La versión %1 de Dofus que tienes instalada no es compatible con este servidor. Para poder jugar, instala la versión %2. El cliente DOFUS se va a cerrar.");
                                            cerrar_Socket();
                                        break;

                                        case 'b':
                                            cuenta.logger.log_Error("Login", "Conexión rechazada. Tu cuenta ha sido baneada.");
                                            cerrar_Socket();
                                        break;

                                        case 'k':
                                            string[] informacion_ban = paquete.Substring(3).Split('|');
                                            int dias = int.Parse(informacion_ban[0].Substring(1)), horas = int.Parse(informacion_ban[1]), minutos = int.Parse(informacion_ban[2]);
                                            StringBuilder mensaje = new StringBuilder().Append("Tu cuenta estará inválida durante ");
                                            if (dias > 0)
                                                mensaje.Append(dias + " días");
                                            if(horas > 0)
                                                mensaje.Append(horas + " horas");
                                            if (minutos > 0)
                                                mensaje.Append(minutos + " minutos");
                                            cuenta.logger.log_Error("Login", mensaje.ToString());
                                            cerrar_Socket();
                                        break;
                                    }
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine("Paquete desconocido: " + paquete);
                        break;
                    }
                    break;
            }
        }

        public void conectar_Game_Server(bool tiene_error, string paquete)
        {
            if (!tiene_error)
            {
                string ip = Hash.desencriptar_IP(paquete);
                int puerto = Compressor.desencriptar_puerto(paquete.Substring(8, 3).ToCharArray());
                cuenta.tiquet_game = paquete.Substring(11);

                cerrar_Socket();
                cuenta.cambiando_Al_Servidor_Juego(ip, puerto);
            }
            else
            {
                switch (paquete[2])
                {
                    case 'E':
                        switch (paquete.Substring(3))
                        {
                            case "f":
                                cuenta.logger.log_Error("Login", "El acceso a este servidor está restringido a una determinada comunidad de jugadores o a todos los abonados. No puedes acceder a él sin cumplir uno de estos requisitos.");
                                cerrar_Socket();
                            break;
                        }
                    break;
                }
            }
        }
    }
}
