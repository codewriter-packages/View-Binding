using System.Collections.Generic;
using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeField]
        private List<ViewBindingBehaviour> listeners = new List<ViewBindingBehaviour>();

        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        [SerializeReference]
        private List<ViewEvent> evts = new List<ViewEvent>();

        [SerializeField]
        private bool renderOnStart = false;

        private LifetimeController _lifetimeController;
        private Atom<object> _render;

        internal List<ViewBindingBehaviour> Listeners => listeners;

        protected internal override int VariablesCount => vars.Count;
        protected internal override int EventCount => evts.Count;

        protected internal override ViewVariable GetVariable(int index) => vars[index];
        protected internal override ViewEvent GetEvent(int index) => evts[index];

        protected virtual void Start()
        {
            if (renderOnStart)
            {
                using (Atom.NoWatch)
                {
                    Render();
                }
            }
        }

        protected virtual void OnDestroy()
        {
            _render = null;

            _lifetimeController?.Dispose();
            _lifetimeController = null;
        }

        private void InitIfNeed()
        {
            if (_lifetimeController != null)
            {
                return;
            }

            using (Atom.NoWatch)
            {
                _lifetimeController = new LifetimeController();

                foreach (var listener in listeners)
                {
                    if (listener == null)
                    {
                        continue;
                    }

                    listener.Setup(_lifetimeController.Lifetime);
                }

                _render = Atom.Computed<object>(_lifetimeController.Lifetime, () =>
                {
                    foreach (var listener in listeners)
                    {
                        if (listener == null)
                        {
                            continue;
                        }

                        listener.LinkToRender();
                    }

                    return null;
                }, debugName: name, keepAlive: true);
            }
        }

        protected internal override void LinkToRender()
        {
            base.LinkToRender();

            _render.Get();
        }

        protected void UnsafeRegisterVariable(ViewVariable variable)
        {
            vars.Add(variable);
        }

        public void Render()
        {
            InitIfNeed();
            LinkToRender();
        }

        internal void FillListeners()
        {
            listeners.Clear();
            listeners.AddRange(gameObject.GetComponentsInChildren<ViewBindingBehaviour>(true));
            listeners.RemoveAll(it =>
                it == null ||
                it is ViewContext ||
                it.GetComponentInParent<ViewContext>() != this);
        }
    }
}