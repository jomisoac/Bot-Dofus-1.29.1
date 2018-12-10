using System;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;

namespace Bot_Dofus_1._29._1.Protocolo.Game
{
    internal class Game : ClienteProtocolo
    {
        private readonly Cuenta cuenta = null;

        public Game(string ip, int puerto, Cuenta _cuenta) : base(ip, puerto)
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
                        case 'G':
                            enviar_Paquete(new TiquetMensaje(cuenta.tiquet_game).get_Mensaje());
                        break;

                        default:
                            Console.WriteLine("Paquete desconocido: " + paquete);
                        break;
                    }
                break;

                case 'A':
                    switch (accion)
                    {
                        case 'T':
                            enviar_Paquete(new TiquetRespuestaMensaje(informacion_paquete).get_Mensaje());
                            enviar_Paquete(new RegionalVersionMensaje().get_Mensaje());
                        break;

                        case 'V':
                            string[] idiomas = { "es", "fr", "en", "pt"};
                            enviar_Paquete("Ag" + idiomas[new Random().Next(0, (idiomas.Length - 1))]);
                            enviar_Paquete("AL");
                            enviar_Paquete("Af");
                        break;

                        case 'L':
                            switch (paquete[2])
                            {
                                case 'K':
                                    enviar_Paquete(new PersonajeSeleccionMensaje(0, informacion_paquete).get_Mensaje());
                                break;
                            }
                        break;

                        case 'S':
                            switch (paquete[2])
                            {
                                case 'K':
                                    enviar_Paquete(new PersonajeSeleccionadoMensaje(paquete.Substring(4), cuenta).get_Mensaje());
                                    cuenta.Fase_Socket = EstadoSocket.JUEGO;
                                break;
                            }
                        break;

                        case 'R'://Restricciones
                            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                        break;
                    }
                break;

                case 'I':
                    switch(accion)
                    {
                        case 'm'://Mensajes por lang
                            byte tipo_im = byte.Parse(paquete[2].ToString());
                            int numero_im = int.Parse(paquete.Substring(3).Split(';')[0]);
                            switch (tipo_im)
                            {
                                case 1://ERROR
                                    switch(numero_im)
                                    {
                                        case 89:
                                            cuenta.logger.log_Error("DOFUS", "¡Bienvenido(a) a DOFUS, el Mundo de los Doce! Atención: Está prohibido comunicar tu usuario de cuenta y tu contraseña.");
                                        break;
                                    }
                                break;
                            
                                case 0://Informacion
                                    switch(numero_im)
                                    {
                                        case 152:
                                            string mensaje = paquete.Substring(3).Split(';')[1];
                                            cuenta.logger.log_informacion("DOFUS", "Última conexión a tu cuenta realizada el " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " a las " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + "mediante la dirección IP " + mensaje.Split('~')[5]);
                                        break;

                                        case 153:
                                            cuenta.logger.log_informacion("DOFUS", "Tu dirección IP actual es " + paquete.Substring(3).Split(';')[1]);
                                            cuenta.personaje.evento_Personaje_Seleccionado();
                                        break;
                                    }
                                break;
                            }
                        break;
                    }
                break;
            }
        }
    }
}
