using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs
{
    class CerrarDialogoAccion : AccionesScript
    {
        internal override Task<ResultadosAcciones> proceso(Account cuenta)
        {
            if (cuenta.esta_dialogando())
            {
                cuenta.connexion.enviar_Paquete("DV", true);
                return resultado_procesado;
            }

            return resultado_hecho;
        }
    }
}
