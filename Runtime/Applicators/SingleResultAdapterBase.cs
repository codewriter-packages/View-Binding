using UniMob;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeWriter.ViewBinding.Applicators
{
    public abstract class SingleResultAdapterBase<TResult, TResultVariable> : ViewContextBase
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
        where TResultVariable : ViewVariable<TResult, TResultVariable>, new()
    {
        [SerializeField]
        [FormerlySerializedAs("resultName")]
        private string alias = "Result";

        [SerializeField]
        [HideInInspector]
        private TResultVariable result;

        public bool IsDestroyed => this == null;

        protected override int VariablesCount => 1;
        protected override int EventCount => 0;

        protected override ViewVariable GetVariable(int index) => result;
        protected override ViewEvent GetEvent(int index) => null;

        protected override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            if (!lifetime.IsDisposed)
            {
                result.SetSource(Atom.Computed(lifetime, Adapt));
                lifetime.Register(() => result.SetSource(null));
            }
        }

        protected abstract TResult Adapt();

        protected abstract bool IsVariableUsed(ViewVariable variable);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (result == null)
            {
                result = new TResultVariable();
            }

            result.SetContext(this);
            result.SetName(alias);

            if (!Application.isPlaying)
            {
                result.SetValueEditorOnly(Adapt());
            }
        }

        public void OnEditorContextVariableChanged(ViewVariable variable)
        {
            if (IsVariableUsed(variable))
            {
                result.SetValueEditorOnly(Adapt());
                NotifyEditorVariableChanged(result);
            }
        }
#endif
    }
}