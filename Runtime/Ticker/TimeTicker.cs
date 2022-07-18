using Elysium.Core.Timers;
using System;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class TimeTicker : ITicker, IDisposable
    {
        private ITimer timer = default;

        public event UnityAction OnTick = delegate { };

        public TimeTicker(TimeSpan _tickInterval)
        {
            timer = Timer.CreateScaledTimer(_tickInterval);
            timer.OnEnd.AddListener(Tick);
            timer.AutoRestart = true;
            timer.Start();
        }

        private void Tick()
        {
            OnTick?.Invoke();
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
