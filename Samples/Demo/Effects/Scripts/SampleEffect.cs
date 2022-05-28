using System;
using UnityEngine;

namespace Elysium.Effects.Samples
{
    public class SampleEffect : IEffect
    {
        public Guid EffectID { get; set; } = Guid.NewGuid();
        public Guid InstanceID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Sample Effect";
        public Sprite Icon { get; set; } = null;
        public int Duration { get; set; } = 10;
        public int Priority { get; set; } = 0;
        public int MaxStack { get; set; } = 1;
        public int MaxVisualStack => MaxStack;
        public bool IsBeneficial { get; set; } = false;
        public bool PersistsOnDeath { get; set; } = false;
        public bool RefreshOnApply { get; set; } = true;

        public SampleEffect()
        {
            
        }

        public bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied, int _totalStacks)
        {
            return true;
        }

        public bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved, int _totalStacks)
        {
            return true;
        }

        public void End(IEffectReceiver _receiver, int _stacks)
        {
            
        }

        public void Tick(IEffectReceiver _receiver, int _stacks)
        {
            
        }

        public override bool Equals(System.Object _effect)
        {
            SampleEffect effect = _effect as SampleEffect;
            if (effect == null) { return false; }
            return effect.EffectID == EffectID && effect.InstanceID == InstanceID;
        }

        public override int GetHashCode()
        {
            return InstanceID.GetHashCode();
        }
    }
}
