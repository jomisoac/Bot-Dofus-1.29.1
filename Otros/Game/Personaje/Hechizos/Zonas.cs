using System;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Hechizos
{
    public class Zonas
    {
        public HechizoZona tipo { get; set; }
        public int tamano { get; set; }

        public Zonas(HechizoZona _tipo, int _tamano)
        {
            tipo = _tipo;
            tamano = _tamano;
        }

        public static Zonas Parse(string str)
        {
            if (str.Length != 2)
                throw new ArgumentException("zona invalida");

            HechizoZona tipo;

            switch (str[0])
            {
                case 'P':
                    tipo = HechizoZona.SOLO;
                break;

                case 'C':
                    tipo = HechizoZona.CIRCULO;
                break;

                case 'L':
                    tipo = HechizoZona.LINEA;
                break;

                case 'X':
                    tipo = HechizoZona.CRUZADO;
                break;

                case 'O':
                    tipo = HechizoZona.ANILLO;
                break;

                case 'R':
                    tipo = HechizoZona.RECTANGULO;
                break;

                case 'T':
                    tipo = HechizoZona.TLINEA;
                break;

                default:
                    tipo = HechizoZona.SOLO;
                break;
            }
            return new Zonas(tipo, Hash.get_Hash(str[1]));
        }
    }
}
