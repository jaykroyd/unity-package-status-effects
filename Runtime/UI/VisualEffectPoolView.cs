using Elysium.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Elysium.Effects.UI
{
    [System.Serializable]
    public class VisualEffectPoolView : PoolView<VisualEffect>, IVisualEffectView
    {
        public new IEnumerable<IVisualEffect> Set(int _numOfSlots)
        {
            return base.Set(_numOfSlots).Cast<IVisualEffect>();
        }
    }
}
