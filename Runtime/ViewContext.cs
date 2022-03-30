using System.Collections.Generic;
using System.Linq;
using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeField]
        private List<ViewBindingBehaviour> listeners = new List<ViewBindingBehaviour>();

        [HideInInspector]
        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        [HideInInspector]
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

        protected virtual void Awake()
        {
            _lifetimeController?.Dispose();
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
            LinkToRender();
        }

        public ViewVariable FindVariable(string variableName) => FindVariable<ViewVariable>(variableName);

        public TViewVariable FindVariable<TViewVariable>(string variableName)
            where TViewVariable : ViewVariable
        {
            foreach (var variable in vars)
            {
                if (variable.Name == variableName && variable is TViewVariable tVariable)
                {
                    return tVariable;
                }
            }

            return null;
        }

        public ViewEvent FindEvent(string eventName) => FindEvent<ViewEvent>(eventName);

        public TViewEvent FindEvent<TViewEvent>(string eventName)
            where TViewEvent : ViewEvent
        {
            foreach (var evt in evts)
            {
                if (evt.Name == eventName && evt is TViewEvent tVariable)
                {
                    return tVariable;
                }
            }

            return null;
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