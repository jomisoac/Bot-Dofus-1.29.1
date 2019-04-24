using System;
using System.Threading;

namespace Bot_Dofus_1._29._1.Utilidades
{
    class TimerWrapper : IDisposable
    {
        private Timer timer;
        public bool habilitado { get; private set; }
        public int intervalo { get; set; }

        public TimerWrapper(int interval, TimerCallback callback)
        {
            intervalo = interval;
            timer = new Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start(bool rightAway = false)
        {
            if (habilitado)
                return;

            habilitado = true;
            timer.Change(rightAway ? 0 : intervalo, intervalo);
        }

        public void Stop()
        {
            if (!habilitado)
                return;

            habilitado = false;
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Dispose()
        {
            timer?.Dispose();
            timer = null;
        }
    }
}
