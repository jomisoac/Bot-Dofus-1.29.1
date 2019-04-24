using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    internal class RecoleccionAccion : AccionesScript
    {
        public List<short> elementos { get; private set; }

        public RecoleccionAccion(List<short> _elementos) => elementos = _elementos;

        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            Personaje personaje = cuenta.personaje;

            if (personaje.mapa.get_Puede_Recolectar_Elementos_Interactivos(elementos))
            {
                bool puede_recolectar = await personaje.mapa.Recolectar(elementos);

                if (!puede_recolectar)
                    return await resultado_fallado;

                return await resultado_procesado;
            }
            return await resultado_hecho;
        }
    }
}
