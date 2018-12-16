using System;

namespace Bot_Dofus_1._29._1.Utilidades.Criptografia
{
    internal class Compressor
    {
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

        public static int index_Hash(char ch)
        {
            for (int i = 0; i < Hash.caracteres_array.Length; i++)
                if (Hash.caracteres_array[i] == ch)
                    return i;
            return -1;
        }
    }
}
