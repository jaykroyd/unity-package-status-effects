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
        int Level { get; }
        int MaxStack { get; }
        int MaxVisualStack { get; }
        bool IsBeneficial { get; }
        bool PersistsOnDeath { get; }
        int Duration { get; }
        bool RefreshOnApply { get; }

        bool ApplyFirst(IEffectApplier _applier, IEffectReceiver _receiver, int _stacks);
        bool ApplyRefresh(IEffectApplier _applier, IEffectReceiver _receiver, int _totalStacksBefore, int _stacksApplied, int _totalStacksAfter);        
        void Tick(IEffectReceiver _receiver, int _stacks);
        bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _totalStacksBefore, int _stacksRemoved, int _totalStacksAfter);
        void EndExpire(IEffectReceiver _receiver, int _stacks);
        void EndReplace(IEffectApplier _applier, IEffectReceiver _receiver, IEffect _new, int _totalStacksBefore, int _stacksApplied, int _totalStacksAfter);              
    }
}