using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public interface IEffectReceiver
    {
        IList<IEffectStack> Stacks { get; }

        event UnityAction OnValueChanged;

        bool Apply(IEffectApplier _applier, IEffect _effect, int _stacks);
        bool Cleanse(IEffectApplier _applier, IEffect _effect, int _stacks);
        bool Cleanse(IEffectApplier _applier, IEffect _effect);
        void CleanseAll(IEffectApplier _applier);
        bool Contains(IEffect _effect);
        int Quantity(IEffect _effect);
    }
}