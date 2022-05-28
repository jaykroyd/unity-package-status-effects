using System;
using System.Collections;
using UnityEngine;

namespace Elysium.Effects
{
    public interface IEffect
    {
        Guid EffectID { get; }
        Guid InstanceID { get; }
        string Name { get; }
        Sprite Icon { get; }
        int Priority { get; }
        int MaxStack { get; }
        int MaxVisualStack { get; }
        bool IsBeneficial { get; }
        bool PersistsOnDeath { get; }
        int Duration { get; }
        bool RefreshOnApply { get; }

        bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied, int _totalStacks);
        void Tick(IEffectReceiver _receiver, int _stacks);
        void End(IEffectReceiver _receiver, int _stacks);
        bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved, int _totalStacks);
    }
}