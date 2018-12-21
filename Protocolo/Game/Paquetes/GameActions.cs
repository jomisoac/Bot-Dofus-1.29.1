using System;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
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
                            else if (cuenta.personaje.mapa.get_Monstruos().ContainsKey(_loc6))
                            {
                                cuenta.personaje.mapa.get_Monstruos()[_loc6] = casilla_destino;
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