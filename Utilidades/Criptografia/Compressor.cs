using System;

namespace Bot_Dofus_1._29._1.Utilidades.Criptografia
{
    class Compressor
    {
        public static char[] caracteres_array = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
            'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        public static int desencriptar_puerto(char[] chars)
        {
            if (chars.Length != 3)
                throw new ArgumentOutOfRangeException("El puerto debe estar encriptado en 3 caracteres.");
            int puerto = 0;
            for (int i = 0; i < 2; ++i)
                puerto += (int)(Math.Pow(64, 2 - i) * index_Hash(chars[i]));
            puerto += index_Hash(chars[2]);
            return puerto;
        }

        private static int index_Hash(char ch)
        {
            for (int i = 0; i < caracteres_array.Length; i++)
                if (caracteres_array[i] == ch)
                    return i;
            return -1;
        }
    }
}
