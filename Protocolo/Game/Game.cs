using System;
using System.Linq;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
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
            string accion = paquete[1].ToString();
            string informacion_paquete = string.Empty;
            bool tiene_error = false;

            if (paquete.Length >= 3)
            {
                informacion_paquete = paquete.Substring(3);
                tiene_error = paquete[2].ToString().Equals("E");
            }

            switch (paquete[0])
            {
                case 'H':
                    switch (accion)
                    {
                        case "G":
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
                        case "T":
                            enviar_Paquete(new TiquetRespuesta(informacion_paquete).get_Mensaje());
                            enviar_Paquete(new RegionalVersion().get_Mensaje());
                        break;

                        case "V":
                            string[] idiomas = { "es", "fr", "en", "pt"};
                            enviar_Paquete("Ag" + idiomas[new Random().Next(0, (idiomas.Length - 1))]);
                            enviar_Paquete("AL");
                            enviar_Paquete("Af");
                        break;

                        case "L":
                            switch (paquete[2])
                            {
                                case 'K':
                                    enviar_Paquete(new PersonajeSeleccion(cuenta.cuenta_configuracion.id_personaje, informacion_paquete).get_Mensaje());
                                break;
                            }
                        break;

                        case "S":
                            switch (paquete[2])
                            {
                                case 'K':
                                    enviar_Paquete(new PersonajeSeleccionado(paquete.Substring(4), cuenta).get_Mensaje());
                                    cuenta.Fase_Socket = EstadoSocket.JUEGO;
                                break;
                            }
                        break;

                        case "R"://Restricciones
                            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                        break;

                        case "s":
                            cuenta.personaje.actualizar_Caracteristicas(paquete);
                        break;
                    }
                break;

                case 'c':
                    switch(accion)
                    {
                        case "C":
                            switch(paquete[2])
                            {
                                case '+':
                                    cuenta.personaje.agregar_Canal_Personaje(informacion_paquete);
                                break;

                                case '-':
                                    cuenta.personaje.eliminar_Canal_Personaje(informacion_paquete);
                                break;
                            }
                        break;

                        case "M":
                            switch (paquete[2])
                            {
                                case 'K':
                                    cuenta.logger.log_normal(string.Empty, paquete.Substring(4).Split('|')[1] + ": " + paquete.Substring(4).Split('|')[2]);
                                break;
                            }
                        break;
                    }
                break;

                case 'B':
                    switch(accion)
                    {
                        case "D":
                            enviar_Paquete("GI");
                        break;
                    }
                break;

                case 'f':
                    switch(accion)
                    {
                        case "C":
                            switch (int.Parse(paquete[2].ToString()))
                            {
                                case 0:
                                    enviar_Paquete("BD");
                                break;
                            }
                        break;
                    }
                break;

                case 'G':
                    switch(accion)
                    {
                        case "A":
                            switch (paquete[2])
                            {
                                case 'S':
                                    //this.aks.GameActions.onActionsStart(sData.substr(3));
                                break;

                                case 'F':
                                    //aks.GameActions.onActionsFinish(sData.substr(3));
                                break;

                                default:
                                    new GameActions(cuenta).get_On_GameAction(paquete.Substring(2));
                                break;
                            }
                        break;

                        case "D":
                            switch(paquete[2].ToString())
                            {
                                case "M"://Mapas
                                    cuenta.personaje.mapa = new Mapa(cuenta, paquete.Substring(4));
                                    cuenta.personaje.evento_Mapa_Actualizado();
                                break;

                                default:
                                    Console.WriteLine("Paquete desconocido: " + paquete);
                                break;
                            }
                        break;

                        case "M":
                            switch (paquete[3])
                            {
                                case '+':
                                    string[] separador_jugadores = paquete.Substring(4).Split('|');
                                    
                                    for (int i = 0; i < separador_jugadores.Length; ++i)
                                    {
                                        string[] gm_elemento = separador_jugadores[i].Split(';');
                                        string tipos = gm_elemento[5];
                                        int tipo_id = 0;
                                        if (tipos.Contains(','))
                                            tipo_id = int.Parse(tipos.Split(',')[0]);
                                        else
                                            tipo_id = int.Parse(tipos);

                                        int celda_id = int.Parse(gm_elemento[0].ToString());
                                        int id_elemento = int.Parse(gm_elemento[3].ToString());
                                        string nombre_personaje = gm_elemento[4].ToString();

                                        if (tipo_id > 0)
                                        {
                                            if (cuenta.personaje.id == id_elemento)
                                                cuenta.personaje.celda_id = celda_id;
                                            cuenta.personaje.mapa.agregar_Personaje(new Personaje(id_elemento, nombre_personaje, byte.Parse(gm_elemento[7].ToString())));
                                        }
                                        if(tipo_id == -3)//monstruos
                                        {
                                            cuenta.personaje.mapa.agregar_Monstruo(id_elemento, celda_id);
                                        }
                                    }
                                break;

                                case '-':
                                    cuenta.personaje.mapa.eliminar_Personaje(int.Parse(paquete.Substring(4).ToString()));
                                break;
                            }
                        break;
                    }
                break;

                case 'S':
                    switch(accion)
                    {
                        case "L":
                            if (!paquete[2].Equals('o'))
                            {
                                cuenta.personaje.actualizar_Hechizos(paquete.Substring(2));
                            }
                        break;
                    }
                break;

                case 'I':
                    switch(accion)
                    {
                        case "m"://Mensajes por lang
                            byte tipo_im = byte.Parse(paquete[2].ToString().ToString());
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
                                            cuenta.logger.log_informacion("DOFUS", "Última conexión a tu cuenta realizada el " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " a las " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + " mediante la dirección IP " + mensaje.Split('~')[5]);
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
