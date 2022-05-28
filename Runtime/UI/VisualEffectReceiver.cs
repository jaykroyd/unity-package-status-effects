using System.Linq;
using UnityEngine;

namespace Elysium.Effects.UI
{
    public class VisualEffectReceiver : IVisualEffectReceiver
    {        
        protected IVisualEffectView view = default;
        protected IEffectReceiver receiver = default;
        protected GameObject panel = default;
        protected bool open = false;

        public VisualEffectReceiver(IEffectReceiver _receiver, IVisualEffectView _view, GameObject _panel)
        {
            this.receiver = _receiver;
            this.view = _view;
            this.panel = _panel;
        }

        public void Show()
        {
            if (open) { return; }
            panel.SetActive(true);
            Register();
            Spawn();
            open = true;
        }

        public void Hide()
        {
            if (!open) { return; }
            Deregister();
            view.Set(0);
            panel.SetActive(false);
            open = false;
        }

        protected virtual void Register()
        {
            receiver.OnValueChanged += Spawn;
        }

        protected virtual void Deregister()
        {
            receiver.OnValueChanged -= Spawn;
        }

        protected void Spawn()
        {            
            var stacks = receiver.Stacks.Where(x => !x.IsEmpty).ToArray();
            int numOfSlots = stacks.Count();
            var objs = view.Set(numOfSlots);
            for (int i = 0; i < numOfSlots; i++)
            {
                IVisualEffect visual = objs.ElementAt(i);
                IEffectStack stack = stacks.ElementAt(i);
                ConfigureSlot(visual, stack);
            }
        }

        protected virtual void ConfigureSlot(IVisualEffect _visual, IEffectStack _stack)
        {
            _visual.Setup(_stack);
        }
    }
}
