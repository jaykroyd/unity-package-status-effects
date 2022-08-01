using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elysium.Effects.UI
{
    public class VisualEffect : MonoBehaviour, IVisualEffect
    {
        [Header("Icon")]
        [SerializeField] protected Image icon = default;

        [Header("Count")]
        [SerializeField] protected TMP_Text stackAmountText = default;
        [SerializeField] protected GameObject stackAmountBackground = default;

        private Sprite defaultSprite = default;

        protected IEffectStack stack = default;

        private void Awake()
        {
            defaultSprite = icon.sprite;
        }

        public void Setup(IEffectStack _stack)
        {
            this.stack = _stack;

            gameObject.name = $"[{stack.GetType().Name}] {stack}";
            SetupQuantity(stack.Stacks);
            SetupIcon(stack.Effect.Icon);
        }

        protected virtual void SetupIcon(Sprite _icon)
        {
            icon.sprite = _icon != null ? _icon : defaultSprite;
        }

        protected virtual void SetupQuantity(int _count)
        {
            stackAmountText.text = $"{_count}";
            stackAmountBackground.SetActive(_count > 1);
        }
    }
}
