using UniMob;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : ViewBindingBehaviour
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        private Reaction _reaction;

        protected override void Start()
        {
            base.Start();

            _reaction = Atom.Reaction(Apply, debugName: name);
        }

        protected override void OnDestroy()
        {
            _reaction?.Deactivate();

            base.OnDestroy();
        }

        protected abstract void Apply();

#if UNITY_EDITOR
        public abstract void OnEditorContextVariableChanged(ViewVariable variable);
#endif
    }
}