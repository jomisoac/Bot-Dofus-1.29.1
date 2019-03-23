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
            //int _loc7_ = int.Parse(_loc6_[0]);
            //int _loc8_ = int.Parse(_loc6_[1]);

            for(int i = 2; i < _loc6_.Length; ++i)
            {
                string[] _loc11_ = _loc6_[i].Split(';');
                lista_personajes _c;
                _c.id = int.Parse(_loc11_[0]);
                _c.nombre = _loc11_[1];
                lista_personaje.Add((i - 2), _c);
                /*
				_loc12_.level = _loc11_ [2];
				_loc12_.gfxID = _loc11_ [3];
				_loc12_.color1 = _loc11_ [4];
				_loc12_.color2 = _loc11_ [5];
				_loc12_.color3 = _loc11_ [6];
				_loc12_.accessories = _loc11_ [7];
				_loc12_.merchant = _loc11_ [8];
				_loc12_.serverID = _loc11_ [9];
				_loc12_.isDead = _loc11_ [10];
				_loc12_.deathCount = _loc11_ [11];
				_loc12_.lvlMax = _loc11_ [12];
				var _loc15_ = this.api.kernel.CharactersManager.createCharacter (_loc13_, _loc14_, _loc12_);
				_loc15_.sortID = Number (_loc13_);
				_loc5_.push (_loc15_);
				_loc9_.push (Number (_loc13_));
				*/
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
