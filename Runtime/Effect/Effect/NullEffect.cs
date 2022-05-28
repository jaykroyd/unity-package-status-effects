using System;
using UnityEngine;

namespace Elysium.Effects
{
    public class NullEffect : IEffect
    {
        public Guid EffectID { get; set; } = Guid.Empty;
        public Guid InstanceID { get; set; } = Guid.Empty;
        public string Name { get; set; } = "null";
        public Sprite Icon { get; set; } = Resources.Load<Sprite>("empty");
        public int Duration { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public int MaxStack { get; set; } = 0;
        public int MaxVisualStack => MaxStack;
        public bool IsBeneficial { get; set; } = false;
        public bool PersistsOnDeath { get; set; } = false;
        public bool RefreshOnApply { get; set; } = true;

        public NullEffect()
        {

        }

        public bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied, int _totalStacks)
        {
            return false;
        }

        public bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved, int _totalStacks)
        {
            return false;
        }

        public void End(IEffectReceiver _receiver, int _stacks)
        {
            
        }

        public void Tick(IEffectReceiver _receiver, int _stacks)
        {
            
        }

        public override bool Equals(System.Object _effect)
        {
            return _effect is NullEffect;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}