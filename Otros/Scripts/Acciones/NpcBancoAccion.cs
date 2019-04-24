using Bot_Dofus_1._29._1.Otros.Entidades.Npcs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    class NpcBancoAccion : AccionesScript
    {
        public int npc_id { get; private set; }
        public int respuesta_id { get; private set; }

        public NpcBancoAccion(int _npc_id, int _respuesta_id)
        {
            npc_id = _npc_id;
            respuesta_id = _respuesta_id;
        }

        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            Npcs npc = null;
            Dictionary<int, Npcs> npcs = cuenta.personaje.mapa.get_Npcs();

            if (npc_id < 0)
            {
                int index = (npc_id * -1) - 1;
                if (npcs.Count() <= index)
                    return ResultadosAcciones.FALLO;

                npc = npcs.ElementAt(index).Value;
            }
            else
                npc = npcs.FirstOrDefault(n => n.Value.npc_id == npc_id).Value;

            await cuenta.conexion.enviar_Paquete("DC" + npc.id);
            return ResultadosAcciones.PROCESANDO;
        }
    }
}
