﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects
{
    public class EffectStack : IEffectStack
    {
        private Guid id = default;
        private IEffect effect = default;
        private int count = default;
        private int ticks = default;

        public Guid ID => id;
        public IEffect Effect => !IsEmpty ? effect : new NullEffect();
        public int Count => count;
        public bool HasEnded => ticks >= Effect.Duration;
        public bool IsFull => effect != null && count >= Effect.MaxStack;
        public bool IsEmpty => effect is null;
        public int MinValue => 0;
        public int MaxValue => Effect.MaxStack;
        public int TicksRemaining => Effect.Duration - ticks;

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction OnFull = delegate { };
        public event UnityAction OnEmpty = delegate { };

        public EffectStack(Guid _stackID)
        {
            id = _stackID;
            ticks = 0;
        }

        public static EffectStack New()
        {
            return new EffectStack(Guid.NewGuid());
        }

        public static EffectStack WithEffect(IEffect _effect, int _stacks)
        {
            var stack = new EffectStack(Guid.NewGuid());
            stack.effect = _effect;
            stack.count = Mathf.Clamp(_stacks, stack.MinValue, stack.MaxValue);
            return stack;
        }

        public bool Contains(IEffect _effect)
        {
            return Effect.Equals(_effect);
        }

        public bool Apply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacksApplied)
        {
            int totalStacks = Mathf.Clamp(Count + _stacksApplied, MinValue, MaxValue);
            if (Effect.Apply(_applier, _receiver, _stacksApplied, totalStacks))
            {
                Add(_stacksApplied);
                if (Effect.RefreshOnApply) { ticks = 0; }
                return true;
            }
            return false;
        }

        public void Tick(IEffectReceiver _receiver, int _stacks)
        {
            Effect.Tick(_receiver, _stacks);
            ticks++;
        }

        public bool Cleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacksRemoved)
        {
            if (Effect.Cleanse(_remover, _receiver, _stacksRemoved, _stacksRemoved + Count))
            {
                Remove(_stacksRemoved);
                return true;
            }
            return false;
        }        

        public void Empty()
        {
            var prev = (effect, count, ticks);
            SetInternal(null);
            SetInternal(MinValue);
            HandleEvents(prev);
        }

        private void Add(int _quantity)
        {
            Set(count + _quantity);
        }

        private void Remove(int _quantity)
        {
            Set(count - _quantity);
        }

        private void Set(IEffect _effect, int _stacks)
        {
            var prev = (effect, count, ticks);
            SetInternal(_effect);
            SetInternal(_stacks);
            HandleEvents(prev);
        }

        private void Set(IEffect _effect)
        {
            var prev = (effect, count, ticks);
            SetInternal(_effect);
            HandleEvents(prev);
        }

        private void Set(int _value)
        {
            var prev = (effect, count, ticks);
            SetInternal(_value);
            HandleEvents(prev);
        }        

        private void SetInternal(IEffect _effect)
        {
            effect = _effect;
        }

        private void SetInternal(int _value)
        {
            count = Mathf.Clamp(_value, MinValue, MaxValue);
            if (count <= 0)
            {
                effect = null;
                count = 0;
            }
        }

        private void HandleEvents((IEffect, int, int) _prev)
        {
            if (_prev.Item1 != effect || _prev.Item2 != count || _prev.Item3 != ticks) { OnValueChanged?.Invoke(); }
            if (IsFull) { OnFull?.Invoke(); }
            if (IsEmpty) { OnEmpty?.Invoke(); }
        }

        public new string ToString()
        {
            return !IsEmpty ? $"x{count} {Effect.Name}" : "{empty}";
        }
    }
}