using System;
using Bot_Dofus_1._29._1.Utilities.Crypto;

namespace Bot_Dofus_1._29._1.Otros.Game.Character.Spells
{
    public class Zones
    {
        public SpellActionZone tipo { get; set; }
        public int tamano { get; set; }

        public Zones(SpellActionZone _tipo, int _tamano)
        {
            tipo = _tipo;
            tamano = _tamano;
        }

        public static Zones Parse(string str)
        {
            if (str.Length != 2)
                throw new ArgumentException("Zone invalide");

            SpellActionZone tipo;

            switch (str[0])
            {
                case 'P':
                    tipo = SpellActionZone.SOLO;
                break;

                case 'C':
                    tipo = SpellActionZone.CIRCULO;
                break;

                case 'L':
                    tipo = SpellActionZone.LINEA;
                break;

                case 'X':
                    tipo = SpellActionZone.CRUZADO;
                break;

                case 'O':
                    tipo = SpellActionZone.ANILLO;
                break;

                case 'R':
                    tipo = SpellActionZone.RECTANGULO;
                break;

                case 'T':
                    tipo = SpellActionZone.TLINEA;
                break;

                default:
                    tipo = SpellActionZone.SOLO;
                break;
            }
            return new Zones(tipo, Hash.get_Hash(str[1]));
        }
    }
}
