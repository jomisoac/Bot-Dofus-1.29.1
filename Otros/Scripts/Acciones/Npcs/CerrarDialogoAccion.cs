using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs
{
    class CerrarDialogoAccion : ScriptAction
    {
        internal override Task<ResultadosAcciones> process(Account cuenta)
        {
            if (cuenta.Is_In_Dialog())
            {
                cuenta.connexion.SendPacket("DV", true);
                return resultado_procesado;
            }

            return resultado_hecho;
        }
    }
}
