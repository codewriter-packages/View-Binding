using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding
{
    public abstract class SingleResultAdapterBase : AdapterBase
    {
    }

    [Preserve]
    public abstract class SingleResultAdapterBase<TResult, TResultVariable> : SingleResultAdapterBase
        where TResultVariable : ViewVariable<TResult, TResultVariable>, new()
    {
        [SerializeField]
        [LabelText("Alias")]
        [PropertyOrder(-1)]
        protected TResultVariable result;

        [Preserve]
        public SingleResultAdapterBase()
        {
        }

        protected internal sealed override int VariablesCount => 1;
        protected internal sealed override int EventCount => 0;

        protected internal sealed override ViewVariable GetVariable(int index) => result;
        protected internal sealed override ViewEvent GetEvent(int index) => null;

        protected internal sealed override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            if (!lifetime.IsDisposed)
            {
                result.SetSource(Atom.Computed(lifetime, Adapt));
                lifetime.Register(() => result.SetSource(null));
            }
        }

        protected internal sealed override void AdaptEditorOnly()
        {
            base.AdaptEditorOnly();

#if UNITY_EDITOR
            result.SetValueEditorOnly(Adapt());
#endif
        }

        protected abstract TResult Adapt();

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (result == null)
            {
                result = new TResultVariable();
            }

            result.SetContext(this);
        }
#endif
    }
}