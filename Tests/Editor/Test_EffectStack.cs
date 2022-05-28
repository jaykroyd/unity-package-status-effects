using NUnit.Framework;
using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects.Tests
{
    public class Test_EffectStack
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestApply()
        {
            IEffect effect = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();
            IEffectReceiver receiver = new NullEffectReceiver();

            var stacks = new EffectStack[]
            {
                EffectStack.WithEffect(effect, 1),
                EffectStack.WithEffect(effect, 5555),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;                

                foreach (var number in Enumerable.Range(1, 10).ToList())
                {
                    onValueChangedTriggers = 0;
                    int prev = stack.Count;
                    int expected = Mathf.Clamp(prev + number, 0, effect.MaxStack);

                    Assert.IsTrue(stack.Apply(applier, receiver, number));
                    Assert.AreEqual(expected, stack.Count);
                    Assert.AreEqual(stack.Count != prev ? 1 : 0, onValueChangedTriggers);
                }

                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestCleanse()
        {
            IEffect effect = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();
            IEffectReceiver receiver = new NullEffectReceiver();

            var stacks = new IEffectStack[]
            {
                EffectStack.WithEffect(effect, 0),
                EffectStack.WithEffect(effect, 1),
                EffectStack.WithEffect(effect, 5),
                EffectStack.WithEffect(effect, 346),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                foreach (var number in Enumerable.Range(1, 10).ToList())
                {
                    onValueChangedTriggers = 0;
                    int prev = stack.Count;
                    stack.Cleanse(applier, receiver, number);
                    int expected = Math.Max(0, prev - number);
                    Assert.AreEqual(stack.Count, expected);

                    if (prev != stack.Count)
                    {
                        Assert.AreEqual(1, onValueChangedTriggers);
                    }
                }

                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestEmpty()
        {
            IEffect effect = new GenericEffect { };
            var stacks = new IEffectStack[]
            {
                EffectStack.WithEffect(effect, 1),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                Assert.False(stack.IsEmpty);
                Assert.AreNotEqual(new NullEffect(), stack.Effect);
                Assert.AreNotEqual(0, stack.Count);

                stack.Empty();
                Assert.True(stack.IsEmpty);
                Assert.AreEqual(new NullEffect(), stack.Effect);
                Assert.AreEqual(0, stack.Count);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }
    }    
}