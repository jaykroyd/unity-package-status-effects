using Elysium.Effects.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.Effects.Samples
{
    public class SampleCharacter : MonoBehaviour, IEffectApplier
    {
        private const int FREEZE_STACKS_APPLIED = 1;
        private const int POISON_STACKS_APPLIED = 1;

        [SerializeField] private SampleCharacter opponent = default;
        [SerializeField] private GameObject effectsPanel = default;
        [SerializeField] private VisualEffectPoolView view = default;
        [SerializeField] private Button freezeButton, poisonButton, cleanseButton = default;

        private IEffectReceiver playerEffects = default;
        private IVisualEffectReceiver visualEffects = default;

        private Dictionary<string, IEffect> statusEffects = default;

        public IEffectReceiver PlayerEffects => playerEffects;

        private void Start()
        {
            statusEffects = new Dictionary<string, IEffect>()
            {
                { "freeze", new SampleEffect{ Name = "freeze", Icon = GetIcon("e_freeze") } },
                { "poison", new SampleEffect{ Name = "poison", Icon = GetIcon("e_poison"), MaxStack = 5 } },
            };

            ITicker ticker = new TimeTicker(TimeSpan.FromSeconds(0.5));
            playerEffects = new EffectReceiver(ticker);

            visualEffects = new VisualEffectReceiver(playerEffects, view, effectsPanel);
            visualEffects.Show();

            freezeButton.onClick.AddListener(Freeze);
            freezeButton.GetComponentInChildren<TMP_Text>().text = $"Freeze";

            poisonButton.onClick.AddListener(Poison);
            poisonButton.GetComponentInChildren<TMP_Text>().text = $"Poison";

            cleanseButton.onClick.AddListener(Cleanse);
            cleanseButton.GetComponentInChildren<TMP_Text>().text = $"Cleanse All";
        }

        public void Freeze()
        {
            opponent.PlayerEffects.Apply(this, statusEffects["freeze"], FREEZE_STACKS_APPLIED);
        }

        public void Poison()
        {
            opponent.PlayerEffects.Apply(this, statusEffects["poison"], POISON_STACKS_APPLIED);
        }

        public void Cleanse()
        {
            PlayerEffects.CleanseAll(this);
        }

        private Sprite GetIcon(string _icon)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Demo1/Effects/Textures/{_icon}.png");
            // return AssetDatabase.LoadAssetAtPath<Sprite>($"Packages/com.elysium.effects/Samples/Demo/Effects/Textures/{_icon}.png");
        }
    }
}
