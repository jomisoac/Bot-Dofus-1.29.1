using System;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Peleas;
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
                    bool esta_vivo = _loc6_[1] != "0" ? false : true;
                    int vida_actual = int.Parse(_loc6_[2]);
                    byte pa = byte.Parse(_loc6_[3]);
                    byte pm = byte.Parse(_loc6_[4]);
                    int celda_id = int.Parse(_loc6_[5]);

                    cuenta.personaje.pelea.get_Agregar_Luchador(new Luchadores(id, esta_vivo, vida_actual, pa, pm, celda_id, vida_actual));
                }
            }
        }

        public void onTurnStart(Cuenta cuenta)
        {

        }
    }
}
