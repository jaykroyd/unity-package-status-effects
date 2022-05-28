using NUnit.Framework;
using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Effects.Tests
{
    public class Test_EffectReceiver
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
            IEffect effect = new GenericEffect { MaxStack = 10 };
            IEffectApplier applier = new NullEffectApplier();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(new NullTicker()),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                effectReceiver.OnValueChanged += TriggerOnValueChanged;

                Assert.True(effectReceiver.Apply(applier, effect, 1));
                Assert.AreEqual(1, effectReceiver.Quantity(effect));
                Assert.AreEqual(1, onValueChangedTriggers);

                Assert.True(effectReceiver.Apply(applier, effect, 6));
                Assert.AreEqual(7, effectReceiver.Quantity(effect));
                Assert.AreEqual(2, onValueChangedTriggers);

                effectReceiver.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestRemove()
        {
            IEffect effect = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(new NullTicker()),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                effectReceiver.OnValueChanged += TriggerOnValueChanged;

                Assert.True(effectReceiver.Apply(applier, effect, 1));
                Assert.AreEqual(1, effectReceiver.Quantity(effect));
                Assert.AreEqual(1, onValueChangedTriggers);

                Assert.True(effectReceiver.Cleanse(applier, effect, 1));
                Assert.AreEqual(0, effectReceiver.Quantity(effect));
                Assert.AreEqual(2, onValueChangedTriggers);

                Assert.True(effectReceiver.Apply(applier, effect, 8));
                Assert.AreEqual(8, effectReceiver.Quantity(effect));
                Assert.AreEqual(3, onValueChangedTriggers);

                Assert.True(effectReceiver.Cleanse(applier, effect, 5));
                Assert.AreEqual(3, effectReceiver.Quantity(effect));
                Assert.AreEqual(4, onValueChangedTriggers);

                Assert.True(effectReceiver.Cleanse(applier, effect, 3));
                Assert.AreEqual(0, effectReceiver.Quantity(effect));
                Assert.AreEqual(5, onValueChangedTriggers);

                effectReceiver.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestContains()
        {
            IEffect effect = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(new NullTicker()),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                effectReceiver.OnValueChanged += TriggerOnValueChanged;

                Assert.True(effectReceiver.Apply(applier, effect, 1));
                Assert.True(effectReceiver.Contains(effect));
                Assert.AreEqual(1, onValueChangedTriggers);

                effectReceiver.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestQuantity()
        {
            IEffect effect = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(new NullTicker()),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                effectReceiver.OnValueChanged += TriggerOnValueChanged;

                Assert.True(effectReceiver.Apply(applier, effect, 1));
                Assert.AreEqual(1, effectReceiver.Quantity(effect));
                Assert.AreEqual(1, onValueChangedTriggers);

                Assert.True(effectReceiver.Apply(applier, effect, 3));
                Assert.AreEqual(4, effectReceiver.Quantity(effect));
                Assert.AreEqual(2, onValueChangedTriggers);

                effectReceiver.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestEmpty()
        {
            IEffect effect1 = new GenericEffect { };
            IEffect effect2 = new GenericEffect { };
            IEffect effect3 = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(new NullTicker()),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                effectReceiver.OnValueChanged += TriggerOnValueChanged;

                Assert.True(effectReceiver.Apply(applier, effect1, 4));
                Assert.True(effectReceiver.Apply(applier, effect2, 2));
                Assert.True(effectReceiver.Apply(applier, effect3, 3));
                Assert.AreEqual(3, onValueChangedTriggers);

                effectReceiver.CleanseAll(applier);
                Assert.AreEqual(0, effectReceiver.Quantity(effect1));
                Assert.AreEqual(0, effectReceiver.Quantity(effect2));
                Assert.AreEqual(0, effectReceiver.Quantity(effect3));
                Assert.AreEqual(0, effectReceiver.Stacks.Count());
                Assert.GreaterOrEqual(onValueChangedTriggers, 4);

                effectReceiver.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestEffectTick()
        {
            GenericEffect effect1 = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();
            ManualTicker ticker = new ManualTicker();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(ticker),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onTickTriggers = 0;
                void TriggerOnTick(IEffectReceiver _receiver, int _stacks) 
                {
                    onTickTriggers++;
                    Assert.AreEqual(effectReceiver, _receiver);
                    Assert.AreEqual(2, _stacks);
                }                
                effect1.OnTick.AddListener(TriggerOnTick);

                effectReceiver.Apply(applier, effect1, 2);

                ticker.Tick();
                Assert.AreEqual(1, onTickTriggers);

                ticker.Tick();
                Assert.AreEqual(2, onTickTriggers);

                effect1.OnTick.RemoveAllListeners();
            }
        }

        [Test]
        public void TestEffectApply()
        {
            GenericEffect effect1 = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();
            ITicker ticker = new ManualTicker();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(ticker),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onApplyTriggers = 0;
                void TriggerOnApply(IEffectApplier _applier, IEffectReceiver _receiver, int _stacks, int _totalStacks)
                {
                    onApplyTriggers++;
                    Assert.AreEqual(effectReceiver, _receiver);
                    Assert.AreEqual(2, _stacks);
                }
                effect1.OnApply.AddListener(TriggerOnApply);

                effectReceiver.Apply(applier, effect1, 2);
                Assert.AreEqual(1, onApplyTriggers);

                effect1.OnTick.RemoveAllListeners();
            }
        }

        [Test]
        public void TestEffectEnd()
        {
            GenericEffect effect1 = new GenericEffect { Duration = 1 };
            IEffectApplier applier = new NullEffectApplier();
            ManualTicker ticker = new ManualTicker();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(ticker),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onEndTriggers = 0;
                void TriggerOnEnd(IEffectReceiver _receiver, int _stacks)
                {
                    onEndTriggers++;
                    Assert.AreEqual(effectReceiver, _receiver);
                    Assert.AreEqual(2, _stacks);
                }
                effect1.OnEnd.AddListener(TriggerOnEnd);

                effectReceiver.Apply(applier, effect1, 2);
                ticker.Tick();
                Assert.AreEqual(1, onEndTriggers);

                effect1.OnTick.RemoveAllListeners();
            }
        }

        [Test]
        public void TestEffectCleanse()
        {
            GenericEffect effect1 = new GenericEffect { };
            IEffectApplier applier = new NullEffectApplier();
            ITicker ticker = new ManualTicker();

            var effectReceivers = new IEffectReceiver[]
            {
                new EffectReceiver(ticker),
            };

            foreach (var effectReceiver in effectReceivers)
            {
                int onCleanseTriggers = 0;
                void TriggerOnCleanse(IEffectApplier _remover, IEffectReceiver _receiver, int _stacks, int _totalStacks)
                {
                    onCleanseTriggers++;
                    Assert.AreEqual(effectReceiver, _receiver);
                    Assert.AreEqual(2, _stacks);
                }
                effect1.OnCleanse.AddListener(TriggerOnCleanse);

                effectReceiver.Apply(applier, effect1, 2);
                effectReceiver.Cleanse(applier, effect1, 2);
                Assert.AreEqual(1, onCleanseTriggers);

                effect1.OnTick.RemoveAllListeners();
            }
        }
    }
}