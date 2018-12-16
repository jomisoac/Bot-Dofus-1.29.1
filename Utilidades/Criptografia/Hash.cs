using System.Text;

namespace Bot_Dofus_1._29._1.Utilidades.Criptografia
{
    public class Hash
    {
        public static char[] caracteres_array = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
            'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        public static string encriptar_Password(string password, string key)
        {
            StringBuilder str = new StringBuilder().Append("#1");
            for (int i = 0; i < password.Length; i++)
            {
                char ch = password[i];
                char ch2 = key[i];
                int num2 = ch / 16;
                int num3 = ch % 16;
                int index = (num2 + ch2) % caracteres_array.Length;
                int num5 = (num3 + ch2) % caracteres_array.Length;
                str.Append(caracteres_array[index]).Append(caracteres_array[num5]);
            }
            return str.ToString();
        }

        public static string desencriptar_IP(string paquete)
        {
            string loc8 = paquete.Substring(0, 8), loc9 = paquete.Substring(8, 3), loc7 = paquete.Substring(11);
            StringBuilder loc5 = new StringBuilder();
            int loc12, loc13, loc10;

            for (int loc11 = 0; loc11 < 8; loc11 += 2)
            {
                byte codigo_ascii = (byte)loc8[loc11];
                loc12 = codigo_ascii - 48;
                byte codigo_ascii2 = (byte)loc8[loc11 + 1];
                loc13 = codigo_ascii2 - 48;
                loc10 = (((loc12 & 15) << 4) | (loc13 & 15));
                if (loc11 != 0)
                    loc5.Append(".");
                loc5.Append(loc10);
            }
            return loc5.ToString();
        }

        public static string encriptar_hexadecimal(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("{0:X2}", (int)c);
            return sb.ToString().Trim();
        }
    }
}
