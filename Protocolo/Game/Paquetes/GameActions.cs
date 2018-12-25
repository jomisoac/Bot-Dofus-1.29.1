using System;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    public class GameActions
    {
        private Cuenta cuenta;

        public GameActions(Cuenta _cuenta)
        {
            cuenta = _cuenta;
        }

        public void get_En_Movimiento(string paquete)
        {
            try
            {
                string[] separador_jugadores = paquete.Split('|');

                for (int i = 0; i < separador_jugadores.Length; ++i)
                {
                    string _loc6 = separador_jugadores[i];
                    if (_loc6.Length != 0)
                    {
                        string[] informaciones = _loc6.Substring(1).Split(';');
                        if (_loc6[0].Equals('+'))
                        {
                            int celda_id = int.Parse(informaciones[0]);
                            int id = int.Parse(informaciones[3]);
                            string nombre_template = informaciones[4];
                            string tipo = informaciones[5];
                            if (tipo.Contains(","))
                                tipo = tipo.Split(',')[0];
                            
                            switch (int.Parse(tipo))
                            {
                                case -1:
                                case -2:
                                    if (cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO)
                                    {
                                        cuenta.personaje.pelea.get_Agregar_Luchador(new Luchadores(int.Parse(informaciones[3]), byte.Parse(informaciones[15])));
                                    }
                                break;

                                case -3://monstruos
                                    foreach (string template in nombre_template.Split(','))
                                    {
                                        cuenta.personaje.mapa.agregar_Monstruo(new Monstruo(id, int.Parse(template), celda_id));
                                    }
                                break;

                                case -4://NPC
                                break;

                                case -5:
                                break;

                                case -6:
                                break;

                                case -7:
                                case -8:
                                break;

                                case -9:
                                break;

                                case -10:
                                break;

                                default:
                                    if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                                    {
                                        if (cuenta.personaje.id == id)
                                            cuenta.personaje.celda_id = celda_id;
                                        else
                                            cuenta.personaje.mapa.agregar_Personaje(new Personaje(id, nombre_template, byte.Parse(informaciones[7].ToString()), celda_id));
                                    }
                                    else
                                    {
                                        cuenta.personaje.pelea.get_Agregar_Luchador(new Luchadores(id, byte.Parse(informaciones[24])));
                                    }
                                break;
                            }
                        }
                        else if (_loc6[0].Equals('-'))
                        {
                            int id = int.Parse(_loc6.Substring(1));
                            cuenta.personaje.mapa.eliminar_Personaje(id);
                            cuenta.personaje.mapa.eliminar_Monstruo(id);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                cuenta.logger.log_Error("get_En_Movimiento", e.ToString());
            };
        }

        public void get_On_GameAction(string sExtraData)
        {
            int _loc3 = sExtraData.IndexOf(";");
            sExtraData = sExtraData.Substring(_loc3 + 1);

            _loc3 = sExtraData.IndexOf(";");
            if (_loc3 > 0)
            {
                int _loc5 = int.Parse(sExtraData.Substring(0, _loc3));
                switch (_loc5)
                {
                    case 0:
                        if (GlobalConf.mostrar_mensajes_debug)
                            cuenta.logger.log_informacion("DEBUG", "Movimiento BUG Detectado enviando GI");
                        cuenta.conexion.enviar_Paquete("GI");
                    break;

                    case 1:
                        sExtraData = sExtraData.Substring(_loc3 + 1);
                        _loc3 = sExtraData.IndexOf(";");

                        int _loc6 = int.Parse(sExtraData.Substring(0, _loc3));
                        string _loc7 = sExtraData.Substring(_loc3 + 1);

                        if (!string.IsNullOrEmpty(_loc7))
                        {
                            int casilla_destino = Pathfinding.get_Celda_Numero(cuenta.personaje.mapa.celdas.Length, _loc7.Substring(_loc7.Length - 2));
                            if (_loc6 == cuenta.personaje.id)//encontrar la casilla de destino
                            {
                                if (casilla_destino > 0)
                                {
                                    cuenta.personaje.celda_id = casilla_destino;
                                }
                            }
                            else if(cuenta.personaje.mapa.get_Personajes().ContainsKey(_loc6))
                            {
                                cuenta.personaje.mapa.get_Personajes()[_loc6].celda_id = casilla_destino;
                                if (GlobalConf.mostrar_mensajes_debug)
                                    cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un personaje a la casilla: " + casilla_destino);
                            }
                            else if (cuenta.personaje.mapa.get_Monstruos().ContainsKey(_loc6))
                            {
                                cuenta.personaje.mapa.get_Monstruos()[_loc6][0].celda_id = casilla_destino;
                                if (GlobalConf.mostrar_mensajes_debug)
                                    cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un grupo de monstruo a la casilla: " + casilla_destino);
                            }
                        }
                    break;
                }
            }
        }
    }
}