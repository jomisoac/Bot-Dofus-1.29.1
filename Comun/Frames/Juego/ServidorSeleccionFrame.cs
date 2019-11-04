using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class ServidorSeleccionFrame : Frame
    {
        [PaqueteAtributo("HG")]
        public void bienvenida_Juego(TcpClient cliente, string paquete) => cliente.SendPacket("AT" + cliente.account.gameTicket);

        [PaqueteAtributo("ATK0")]
        public void resultado_Servidor_Seleccion(TcpClient cliente, string paquete)
        {
            cliente.SendPacket("Ak0");
            cliente.SendPacket("AV");
        }

        [PaqueteAtributo("AV0")]
        public void lista_Personajes(TcpClient cliente, string paquete)
        {
            cliente.SendPacket("Ages");
            cliente.SendPacket("AL");
            cliente.SendPacket("Af");
        }

        [PaqueteAtributo("ALK")]
        public void seleccionar_Personaje(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            string[] _loc6_ = paquete.Substring(3).Split('|');
            int contador = 2;
            bool encontrado = false;

            while (contador < _loc6_.Length && !encontrado)
            {
                string[] _loc11_ = _loc6_[contador].Split(';');
                int id = int.Parse(_loc11_[0]);
                string nombre = _loc11_[1];

                if (nombre.ToLower().Equals(cuenta.accountConfig.characterName.ToLower()) || string.IsNullOrEmpty(cuenta.accountConfig.characterName))
                {
                    cliente.SendPacket("AS" + id, true);
                    encontrado = true;
                }

                contador++;
            }
        }

        [PaqueteAtributo("BT")]
        public void get_Tiempo_Servidor(TcpClient cliente, string paquete) => cliente.SendPacket("GI");

        [PaqueteAtributo("ASK")]
        public void personaje_Seleccionado(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            string[] _loc4 = paquete.Substring(4).Split('|');

            int id = int.Parse(_loc4[0]);
            string nombre = _loc4[1];
            byte nivel = byte.Parse(_loc4[2]);
            byte raza_id = byte.Parse(_loc4[3]);
            byte sexo = byte.Parse(_loc4[4]);

            cuenta.game.character.set_Datos_Personaje(id, nombre, nivel, sexo, raza_id);
            cuenta.game.character.inventario.agregar_Objetos(_loc4[9]);

            cliente.SendPacket("GC1");

            cuenta.game.character.evento_Personaje_Seleccionado();
            cuenta.game.character.timer_afk.Change(1200000, 1200000);
            cliente.account.AccountState = AccountStates.CONNECTED_INACTIVE;
        }
    }
}
