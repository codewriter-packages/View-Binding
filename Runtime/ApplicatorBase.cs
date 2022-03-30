using UniMob;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : ViewBindingBehaviour
    {
        private Atom<object> _render;

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
    }
}