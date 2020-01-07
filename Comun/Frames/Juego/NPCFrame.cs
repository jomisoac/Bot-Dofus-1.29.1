using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using System.Collections.Generic;
using System.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class NPCFrame : Frame
    {
        [PaqueteAtributo("DCK")]
        public void get_Dialogo_Creado(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            cuenta.AccountState = AccountStates.DIALOG;
            cuenta.game.character.hablando_npc_id = sbyte.Parse(paquete.Substring(3));
        }

        [PaqueteAtributo("DQ")]
        public void get_Lista_Respuestas(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            if (!cuenta.Is_In_Dialog())
                return;

            IEnumerable<Npcs> npcs = cuenta.game.map.lista_npcs();
            Npcs npc = npcs.ElementAt((cuenta.game.character.hablando_npc_id * -1) - 1);

            if (npc != null)
            {
                string[] pregunta_separada = paquete.Substring(2).Split('|');
                string[] respuestas_disponibles = pregunta_separada[1].Split(';');

                npc.pregunta = short.Parse(pregunta_separada[0].Split(';')[0]);
                npc.respuestas = new List<short>(respuestas_disponibles.Count());

                foreach (string respuesta in respuestas_disponibles)
                    npc.respuestas.Add(short.Parse(respuesta));

                cuenta.game.character.evento_Dialogo_Recibido();
            }
        }
    }
}
