using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : ViewBindingBehaviour
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        private LifetimeController _lifetimeController;

        public bool IsDestroyed => this == null;

        public override void OnContextStart()
        {
            base.OnContextStart();

            if (_lifetimeController != null)
            {
                Debug.LogError($"OnContextStart called multiple times");
            }
            else
            {
                _lifetimeController = new LifetimeController();
                Atom.Reaction(_lifetimeController.Lifetime, Apply, debugName: name);
            }
        }

        public override void OnContextDestroy()
        {
            _lifetimeController?.Dispose();
            _lifetimeController = null;

            base.OnContextDestroy();
        }

        protected abstract void Apply();

#if UNITY_EDITOR
        public abstract void OnEditorContextVariableChanged(ViewVariable variable);
#endif
    }
}