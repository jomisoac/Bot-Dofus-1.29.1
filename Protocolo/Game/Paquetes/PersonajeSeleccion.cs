using System.Collections.Generic;

namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    internal class PersonajeSeleccion
    {
        private readonly int id;
        private readonly Dictionary<int, lista_personajes> lista_personaje;

        public PersonajeSeleccion(int id_seleccionado, string paquete)
        {
            id = id_seleccionado - 1;
            lista_personaje = new Dictionary<int, lista_personajes>();
            string[] _loc6_ = paquete.Split('|');

            for(int i = 2; i < _loc6_.Length; ++i)
            {
                string[] _loc11_ = _loc6_[i].Split(';');
                lista_personajes _c;
                _c.id = int.Parse(_loc11_[0]);
                _c.nombre = _loc11_[1];
                lista_personaje.Add((i - 2), _c);
            }
        }

        public string get_Mensaje()
        {
            return "AS" + lista_personaje[id].id;
        }
    }

    public struct lista_personajes
    {
        public int id;
        public string nombre;
    }
}
