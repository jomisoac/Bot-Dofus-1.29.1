using System;
using System.Threading.Tasks;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;

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

            cuenta.pelea.get_Combate_Creado();
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
                int id = int.Parse(_loc6_[0]);
                Luchadores luchador = cuenta.pelea.get_Luchador_Por_Id(id);
                if (_loc6_.Length != 0)
                {
                    bool esta_vivo = _loc6_[1].Equals("0");
                    if (esta_vivo)
                    {
                        int vida_actual = int.Parse(_loc6_[2]);
                        byte pa = byte.Parse(_loc6_[3]);
                        byte pm = byte.Parse(_loc6_[4]);
                        int celda_id = int.Parse(_loc6_[5]);
                        int vida_maxima = int.Parse(_loc6_[7]);

                        if (celda_id > 0)//son espectadores papa
                        {
                            byte equipo = Convert.ToByte(id > 0 ? 1 : 0);
                            luchador?.get_Actualizar_Luchador(id, esta_vivo, vida_actual, pa, pm, celda_id, vida_maxima, equipo);
                        }
                    }
                    else
                    {
                        luchador?.get_Actualizar_Luchador(id, esta_vivo, 0, 0, 0, -1, 0, 0);
                    }
                }
            }
        }

        public void onTurnStart(Cuenta cuenta, string sExtraData)
        {
            if (int.Parse(sExtraData.Split('|')[0]) == cuenta.personaje.id)
            {
                cuenta.pelea.get_Turno_Iniciado();
            }
        }
    }
}
