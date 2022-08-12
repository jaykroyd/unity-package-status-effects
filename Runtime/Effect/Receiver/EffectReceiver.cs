using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class EffectReceiver<T> : IEffectReceiver<T>, IDisposable
    {
        private List<IEffectStack> stacks = default;
        private ITicker ticker = default;
        private T affected = default;

        public IList<IEffectStack> Stacks => stacks;
        public T Affected => affected;

        public event UnityAction OnValueChanged;
        public event UnityAction<IEffect, int, int> OnEffectAdded;
        public event UnityAction<IEffect, int> OnEffectStacksChanged;
        public event UnityAction<IEffect, int> OnEffectDurationChanged;
        public event UnityAction<IEffect> OnEffectRemoved;

        public EffectReceiver(ITicker _ticker, T _affected)
        {
            this.affected = _affected;
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
            return Stacks.Where(x => x.Contains(_effect)).Sum(x => x.Stacks);
        }

        public bool Apply(IEffectApplier _applier, IEffect _effect, int _stacks)
        {
            if (TryGetEffectStack(_effect, out IEffectStack _current))
            {
                return HandleExistingStack(_applier, _effect, _stacks, _current);
            }

            return HandleNewStack(_applier, _effect, _stacks);
        }

        private bool HandleNewStack(IEffectApplier _applier, IEffect _effect, int _stacks)
        {
            IEffectStack stack = EffectStack.WithEffect(_effect, 0);
            bool applied = stack.Apply(_applier, this, _effect, _stacks);
            if (applied)
            {
                Stacks.Add(stack);
                BindStack(stack);                
                OnEffectAdded?.Invoke(_effect, _stacks, _effect.Duration);
                TriggerOnValueChanged();
            }
            return applied;
        }

        private bool HandleExistingStack(IEffectApplier _applier, IEffect _effect, int _stacks, IEffectStack _current)
        {
            int prevStacks = _current.Stacks;
            int prevTicks = _current.TicksRemaining;
            bool applied = _current.Apply(_applier, this, _effect, _stacks);
            if (applied)
            {
                if (prevStacks != _current.Stacks) { OnEffectStacksChanged?.Invoke(_effect, _current.Stacks); }
                if (prevTicks != _current.TicksRemaining) { OnEffectDurationChanged?.Invoke(_effect, _current.TicksRemaining); }
            }
            return applied;
        }

        public bool Cleanse(IEffectApplier _remover, IEffect _effect, int _stacks)
        {
            if (TryGetEffectStack(_effect, out IEffectStack _current))
            {
                int prevStacks = _current.Stacks;
                int prevTicks = _current.TicksRemaining;
                if (_current.Cleanse(_remover, this, _stacks))
                {
                    if (prevStacks != _current.Stacks) { OnEffectStacksChanged?.Invoke(_effect, _current.Stacks); }
                    if (prevTicks != _current.TicksRemaining) { OnEffectDurationChanged?.Invoke(_effect, _current.TicksRemaining); }
                    return true;
                }
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
                int prevStacks = stack.Stacks;
                int prevTicks = stack.TicksRemaining;
                stack.Tick(this, stack.Stacks);
                if (!stack.HasEnded && prevStacks != stack.Stacks) { OnEffectStacksChanged?.Invoke(stack.Effect, stack.Stacks); }
                if (!stack.HasEnded && prevTicks != stack.TicksRemaining) { OnEffectDurationChanged?.Invoke(stack.Effect, stack.TicksRemaining); }
            }

            for (int i = effects.Count(); i-- > 0;)
            {
                IEffectStack stack = effects.ElementAt(i);
                if (stack.HasEnded) 
                {
                    stack.Effect.EndExpire(this, stack.Stacks);
                    var effect = stack.Effect;
                    stack.Empty();
                    OnEffectRemoved?.Invoke(effect);
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

        public void Dispose()
        {
            foreach (var stack in Stacks)
            {
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }
    }
}