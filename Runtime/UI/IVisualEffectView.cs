using System.Collections.Generic;

namespace Elysium.Effects.UI
{
    public interface IVisualEffectView
    {
        IEnumerable<IVisualEffect> Set(int _numOfSlots);
    }
}
