using System;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public interface IEffectStack
    {
        Guid ID { get; }
        IEffect Effect { get; }
        int Count { get; }
        bool HasEnded { get; }
        bool IsFull { get; }
        bool IsEmpty { get; }        

        event UnityAction OnValueChanged;
        event UnityAction OnFull;
        event UnityAction OnEmpty;

        bool Contains(IEffect _effect);
        bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied);
        void Tick(IEffectReceiver _receiver, int _stacks);        
        bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved);
        void Empty();        
    }
}