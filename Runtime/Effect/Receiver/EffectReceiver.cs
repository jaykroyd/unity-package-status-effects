﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class EffectReceiver : IEffectReceiver
    {
        private List<IEffectStack> stacks = default;
        private ITicker ticker = default;

        public IList<IEffectStack> Stacks => stacks;

        public event UnityAction OnValueChanged = delegate { };

        public EffectReceiver(ITicker _ticker )
        {
            stacks = new List<IEffectStack>();
            stacks.ForEach(x => BindStack(x));

            this.ticker = _ticker;
            ticker.OnTick += Tick;
        }

        public bool Contains(IEffect _effect)
        {
            return Stacks.Any(x => x.Contains(_effect));
        }

        public int Quantity(IEffect _effect)
        {
            return Stacks.Where(x => x.Contains(_effect)).Sum(x => x.Count);
        }

        public bool Apply(IEffectApplier _applier, IEffect _effect, int _stacks)
        {
            // check if effect can be applied
            if (TryGetEffectStack(_effect, out IEffectStack _current))
            {
                return _current.Apply(_applier, this, _stacks);
            }

            IEffectStack stack = EffectStack.WithEffect(_effect, 0);
            if (stack.Apply(_applier, this, _stacks))
            {
                Stacks.Add(stack);
                BindStack(stack);
                TriggerOnValueChanged();
                return true;
            }
            return false;
        }

        public bool Cleanse(IEffectApplier _remover, IEffect _effect, int _stacks)
        {
            if (TryGetEffectStack(_effect, out IEffectStack _current))
            {
                return _current.Cleanse(_remover, this, _stacks);
            }
            return false;
        }

        public bool Cleanse(IEffectApplier _remover, IEffect _effect)
        {
            return Cleanse(_remover, _effect, int.MaxValue);
        }

        public void CleanseAll(IEffectApplier _remover)
        {
            int numOfStacks = Stacks.Count();
            for (int i = numOfStacks; i-- > 0;)
            {
                Cleanse(_remover, Stacks.ElementAt(i).Effect);
            }
        }

        private void Tick()
        {
            var effects = Stacks.OrderBy(x => x.Effect.Priority).ToList();

            for (int i = effects.Count(); i-- > 0;)
            {
                IEffectStack stack = effects.ElementAt(i);
                stack.Tick(this, stack.Count);
            }

            for (int i = effects.Count(); i-- > 0;)
            {
                IEffectStack stack = effects.ElementAt(i);
                if (stack.HasEnded) 
                {
                    stack.Effect.End(this, stack.Count);
                    stack.Empty(); 
                }
            }
        }

        protected bool TryGetEffectStack(IEffect _effect, out IEffectStack _stack)
        {
            _stack = Stacks.FirstOrDefault(x => x.Contains(_effect));
            return _stack != null;
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        protected virtual void BindStack(IEffectStack _stack)
        {
            void CullStack()
            {
                _stack.OnEmpty -= CullStack;
                _stack.OnValueChanged -= TriggerOnValueChanged;
                Stacks.Remove(_stack);
                // TriggerOnValueChanged();
            }

            _stack.OnValueChanged += TriggerOnValueChanged;
            _stack.OnEmpty += CullStack;
        }

        ~EffectReceiver()
        {
            foreach (var stack in Stacks)
            {
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }
    }
}