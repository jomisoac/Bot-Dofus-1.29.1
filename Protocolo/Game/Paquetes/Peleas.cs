using System;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;

namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    public class Peleas
    {
        public void onPositionStart(Cuenta cuenta, string sExtraData)
        {
            var _loc3 = sExtraData.Split('|');
            var _loc4 = _loc3[0];
            var _loc5 = _loc3[1];
            var equipo_actual = int.Parse(_loc3[2]);

            cuenta.Estado_Cuenta = EstadoCuenta.LUCHANDO;
            cuenta.personaje.pelea = new Pelea(cuenta);
            if (cuenta.pelea_extension.configuracion.desactivar_espectador)
                cuenta.conexion.enviar_Paquete("fS");
            cuenta.conexion.enviar_Paquete("GR1");
        }

        public void onTurnMiddle(Cuenta cuenta, string sExtraData)
        {
            string[] separador = sExtraData.Split('|');

            for (int i = 0; i < separador.Length; ++i)
            {
                string[] _loc6_ = separador[i].Split(';');
                if (_loc6_.Length != 0)
                {
                    int id = int.Parse(_loc6_[0]);
                    bool esta_vivo = _loc6_[1].Equals("0");
                    if(esta_vivo)
                    {
                        int vida_actual = int.Parse(_loc6_[2]);
                        byte pa = byte.Parse(_loc6_[3]);
                        byte pm = byte.Parse(_loc6_[4]);
                        int celda_id = int.Parse(_loc6_[5]);
                        int vida_maxima = int.Parse(_loc6_[7]);
                        if(celda_id != -1)//son espectadores papa
                            cuenta.personaje.pelea.get_Luchador_Por_Id(id).get_Actualizar_Luchador(id, esta_vivo, vida_actual, pa, pm, celda_id, vida_maxima);
                    }
                    else
                        cuenta.personaje.pelea.get_Luchador_Por_Id(id).esta_vivo = esta_vivo;

                }
            }
        }

        public void onTurnStart(Cuenta cuenta, string sExtraData)
        {
            if(int.Parse(sExtraData.Split('|')[0]) == cuenta.personaje.id)
            {
                byte id_siguiente_hechizo = 0;
                Mapa mapa = cuenta.personaje.mapa;
                Luchadores luchador = cuenta.personaje.pelea.get_Luchador_Por_Id(cuenta.personaje.id);
                Luchadores enemigo_cercano = cuenta.personaje.pelea.get_Obtener_enemigo_Mas_Cercano();

                if (cuenta.pelea_extension.configuracion.hechizos.Count == 0)
                {
                    cuenta.conexion.enviar_Paquete("Gt");
                    return;
                }

                if (mapa.get_Distancia_Entre_Dos_Casillas(luchador.celda_id, enemigo_cercano.celda_id) < cuenta.pelea_extension.configuracion.celdas_maximas)
                {
                    get_Procesar_hechizo(cuenta, luchador.celda_id, enemigo_cercano.celda_id, ref id_siguiente_hechizo);
                }
                else
                {
                    switch (cuenta.personaje.mapa.get_Mover_Celda_Resultado(luchador.celda_id, enemigo_cercano.celda_id, false, luchador.pm))
                    {
                        case ResultadoMovimientos.EXITO:
                            cuenta.logger.log_informacion("PELEAS", "movimiento de acercamiento exitoso");
                            get_Procesar_hechizo(cuenta, luchador.celda_id, enemigo_cercano.celda_id, ref id_siguiente_hechizo);
                        break;

                        default:
                            cuenta.logger.log_Error("PELEAS", "Fallo al mover el personaje en el combate");
                            cuenta.conexion.enviar_Paquete("GT");
                        break;
                    }
                }
            }
        }

        private void get_Procesar_hechizo(Cuenta cuenta, int celda_personaje, int celda_objetivo, ref byte id_siguiente_hechizo)
        {
            HechizoPelea hechizo_actual = cuenta.pelea_extension.configuracion.hechizos[id_siguiente_hechizo];

            if (cuenta.personaje.pelea.get_Puede_Lanzar_hechizo(hechizo_actual.id) == FallosLanzandoHechizo.NINGUNO)
            {
                switch (cuenta.personaje.pelea.get_Puede_Lanzar_hechizo(hechizo_actual.id))
                {
                    case FallosLanzandoHechizo.NINGUNO:
                        cuenta.personaje.pelea.get_Lanzar_Hechizo(hechizo_actual.id, celda_objetivo);
                        Task.Delay(300).Wait();
                        cuenta.conexion.enviar_Paquete("GKK0");
                        cuenta.conexion.enviar_Paquete("Gt");
                    break;

                    default:
                        cuenta.conexion.enviar_Paquete("Gt");
                    break;
                }
            }
            else
            {
                cuenta.conexion.enviar_Paquete("Gt");
            }
        }
    }
}
