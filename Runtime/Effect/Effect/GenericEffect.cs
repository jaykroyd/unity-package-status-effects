using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class GenericEffect : IEffect
    {
        public Guid EffectID { get; set; } = Guid.NewGuid();
        public Guid InstanceID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Generic";
        public Sprite Icon { get; set; } = null;
        public int Duration { get; set; } = 10;
        public int Priority { get; set; } = 0;
        public int MaxStack { get; set; } = 10;
        public int MaxVisualStack => MaxStack;
        public bool IsBeneficial { get; set; } = false;
        public bool PersistsOnDeath { get; set; } = false;
        public bool CanCleanse { get; set; } = true;
        public bool RefreshOnApply { get; set; } = true;

        public UnityEvent<IEffectApplier, IEffectReceiver, int, int> OnApply = new UnityEvent<IEffectApplier, IEffectReceiver, int, int>();
        public UnityEvent<IEffectApplier, IEffectReceiver, int, int> OnCleanse = new UnityEvent<IEffectApplier, IEffectReceiver, int, int>();
        public UnityEvent<IEffectReceiver, int> OnEnd = new UnityEvent<IEffectReceiver, int>();
        public UnityEvent<IEffectReceiver, int> OnTick = new UnityEvent<IEffectReceiver, int>();

        public GenericEffect()
        {

        }

        public bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied, int _totalStacks)
        {
            OnApply?.Invoke(_applier, _receiver, _stacksApplied, _totalStacks);
            return true;
        }

        public bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved, int _totalStacks)
        {
            if (CanCleanse) { OnCleanse?.Invoke(_remover, _receiver, _stacksRemoved, _totalStacks); }
            return CanCleanse;
        }

        public void End(IEffectReceiver _receiver, int _totalStacks)
        {
            OnEnd?.Invoke(_receiver, _totalStacks);
        }

        public void Tick(IEffectReceiver _receiver, int _stacks)
        {
            OnTick?.Invoke(_receiver, _stacks);
        }

        public override bool Equals(System.Object _effect)
        {
            GenericEffect effect = _effect as GenericEffect;
            if (effect == null) { return false; }
            return effect.EffectID == EffectID && effect.InstanceID == InstanceID;
        }

        public override int GetHashCode()
        {
            return InstanceID.GetHashCode();
        }
    }
}