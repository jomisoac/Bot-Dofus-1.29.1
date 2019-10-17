using System;
using System.Threading;

namespace Bot_Dofus_1._29._1.Utilities
{
    class TimerWrapper : IDisposable
    {
        private Timer timer;
        public bool isEnabled { get; private set; }
        public int interval { get; set; }

        public TimerWrapper(int _interval, TimerCallback callback)
        {
            interval = _interval;
            timer = new Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start(bool immediately = false)
        {
            if (isEnabled)
                return;

            isEnabled = true;
            timer.Change(immediately ? 0 : interval, interval);
        }

        public void Stop()
        {
            if (!isEnabled)
                return;

            isEnabled = false;
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Dispose()
        {
            timer?.Dispose();
            timer = null;
        }
    }
}
