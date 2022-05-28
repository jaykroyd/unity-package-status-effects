using UnityEngine.Events;

namespace Elysium.Effects
{
    public class NullTicker : ITicker
    {
        public event UnityAction OnTick = delegate { };
    }
}
