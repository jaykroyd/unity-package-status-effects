using UnityEngine.Events;

namespace Elysium.Effects
{
    public interface ITicker
    {
        event UnityAction OnTick;
    }
}