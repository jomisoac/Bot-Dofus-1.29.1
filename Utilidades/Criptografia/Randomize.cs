using System;
using System.Threading;

namespace Bot_Dofus_1._29._1.Utilidades.Criptografia
{
    internal class Randomize
    {
        private static int seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        private static readonly object bloqueo = new object();

        public static int get_Random(int minimo, int maximo)
        {
            lock (bloqueo)
            {
                return minimo <= maximo ? random.Value.Next(minimo, maximo) : random.Value.Next(maximo, minimo);
            }
        }

        public static double get_Random_Numero()
        {
            lock (bloqueo)
            {
                return random.Value.NextDouble();
            }
        }
    }
}