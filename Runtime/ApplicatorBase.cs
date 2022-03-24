using UniMob;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : ViewBindingBehaviour
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        private readonly LifetimeController _lifetimeController = new LifetimeController();

        public bool IsDestroyed => this == null;

        protected override void Start()
        {
            base.Start();

            Atom.Reaction(_lifetimeController.Lifetime, Apply, debugName: name);
        }

        protected override void OnDestroy()
        {
            _lifetimeController?.Dispose();

            base.OnDestroy();
        }

        protected abstract void Apply();

#if UNITY_EDITOR
        public abstract void OnEditorContextVariableChanged(ViewVariable variable);
#endif
    }
}