using Bot_Dofus_1._29._1.Otros.Entidades.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs
{
    public class RespuestaAccion : AccionesScript
    {
        public short respuesta_id { get; private set; }

        public RespuestaAccion(short _respuesta_id) => respuesta_id = _respuesta_id;

        internal override Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            if (!cuenta.esta_dialogando())
                return resultado_fallado;

            IEnumerable<Npc> npcs = cuenta.juego.mapa.lista_npcs();
            Npc npc = npcs.ElementAt((cuenta.juego.personaje.hablando_npc_id * -1) - 1);

            if(npc == null)
                return resultado_fallado;

            if (respuesta_id < 0)
            {
                int index = (respuesta_id * -1) - 1;

                if (npc.respuestas.Count <= index)
                    return resultado_fallado;

                respuesta_id = npc.respuestas[index];
            }

            if (npc.respuestas.Contains(respuesta_id))
            {
                cuenta.conexion.enviar_Paquete("DR" + npc.pregunta + "|" + respuesta_id);
                return resultado_procesado;
            }

            return resultado_fallado;
        }
    }
}
