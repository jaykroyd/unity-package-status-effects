using UnityEngine.Events;

namespace Elysium.Effects
{
    public class ManualTicker : ITicker
    {
        public event UnityAction OnTick = delegate { };

        public void Tick()
        {
            OnTick?.Invoke();
        }
    }
}
