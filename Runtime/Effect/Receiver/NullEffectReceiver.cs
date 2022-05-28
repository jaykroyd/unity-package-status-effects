
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class NullEffectReceiver : IEffectReceiver
    {
        private List<IEffectStack> stacks = new List<IEffectStack>();

        public IList<IEffectStack> Stacks => stacks;

        public event UnityAction OnValueChanged = delegate { };

        public bool Apply(IEffectApplier _applier, IEffect _effect, int _stacks)
        {
            return false;
        }

        public bool Cleanse(IEffectApplier _applier, IEffect _effect, int _stacks)
        {
            return false;
        }

        public bool Cleanse(IEffectApplier _applier, IEffect _effect)
        {
            return false;
        }

        public void CleanseAll(IEffectApplier _applier)
        {
            
        }

        public bool Contains(IEffect _effect)
        {
            return false;
        }

        public int Quantity(IEffect _effect)
        {
            return 0;
        }
    }
}