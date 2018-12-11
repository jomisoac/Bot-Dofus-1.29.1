using System;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Login.Paquetes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

namespace Bot_Dofus_1._29._1.Protocolo.Login
{
    public class Login : ClienteProtocolo
    {
        private Cuenta cuenta = null;

        public Login(string ip, int puerto, Cuenta _cuenta) : base(ip, puerto)
        {
            cuenta = _cuenta;
            evento_paquete_recibido += analisis_Paquete;
        }

        public void analisis_Paquete(string paquete)
        {
            char accion = paquete[1];
            string informacion_paquete = string.Empty;
            bool tiene_error = false;

            if (paquete.Length >= 3)
            {
                informacion_paquete = paquete.Substring(3);
                tiene_error = (paquete[2] == 'E');
            }

            switch (paquete[0])
            {
                case 'H':
                    switch (accion)
                    {
                        case 'C':
                            cuenta.Estado_Cuenta = EstadoCuenta.CONECTANDO;
                            cuenta.Fase_Socket = EstadoSocket.LOGIN;
                            cuenta.key_bienvenida = paquete.Substring(2);
                            enviar_Paquete(new VersionMensaje().get_Mensaje());
                            enviar_Paquete(new LoginMensaje(cuenta.usuario, cuenta.password, cuenta.key_bienvenida).get_Mensaje());
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
                            HostsMensaje servidor = new HostsMensaje(informacion_paquete, cuenta.servidor_id);
                            enviar_Paquete(servidor.get_Mensaje());
                            cuenta.logger.log_informacion("Login", "El servidor " + cuenta.get_Nombre_Servidor() + " esta " + (HostsMensaje.EstadosServidor)servidor.estado);
                            break;

                        case 'x':
                            enviar_Paquete(new ListaServidoresMensaje(informacion_paquete, cuenta.servidor_id).get_Mensaje());
                            break;

                        case 'X':
                            conectar_Game_Server(tiene_error, informacion_paquete);
                            cuenta.Fase_Socket = EstadoSocket.CAMBIANDO_A_JUEGO;
                        break;

                        case 'l':
                            switch (paquete[2])
                            {
                                case 'E':
                                    switch (informacion_paquete)
                                    {
                                        case "f":
                                            cuenta.logger.log_Error("Login", "Conexión rechazada. Nombre de cuenta o contraseña incorrectos.");
                                            cerrar_Socket();
                                            break;

                                        case "v":
                                            cuenta.logger.log_Error("Login", "La versión %1 de Dofus que tienes instalada no es compatible con este servidor. Para poder jugar, instala la versión %2. El cliente DOFUS se va a cerrar.");
                                            cerrar_Socket();
                                            break;

                                        case "b":
                                            cuenta.logger.log_Error("Login", "Conexión rechazada. Tu cuenta ha sido baneada.");
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
