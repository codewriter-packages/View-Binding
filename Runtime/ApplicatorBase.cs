using UniMob;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : ViewBindingBehaviour
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        private Atom<object> _render;

        public bool IsDestroyed => this == null;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            _render = Atom.Computed<object>(lifetime, () =>
            {
                Apply();
                return null;
            });
        }

        protected internal override void LinkToRender()
        {
            base.LinkToRender();

            _render.Get();
        }

        protected abstract void Apply();

#if UNITY_EDITOR
        public abstract void OnEditorContextVariableChanged(ViewVariable variable);
#endif
    }
}