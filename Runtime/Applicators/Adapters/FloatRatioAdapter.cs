using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public sealed class FloatRatioAdapter : ViewContextBase
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        [SerializeField]
        private ViewVariableFloat numerator;

        [SerializeField]
        private ViewVariableFloat denominator;

        [SerializeField]
        private string resultName = "Result";

        [SerializeField]
        [HideInInspector]
        private ViewVariableFloat result;

        private readonly LifetimeController _lifetimeController = new LifetimeController();

        protected override int VariablesCount => 1;
        protected override int EventCount => 0;

        protected override ViewVariable GetVariable(int index) => result;
        protected override ViewEvent GetEvent(int index) => null;

        protected override void Start()
        {
            base.Start();

            result.SetSource(Atom.Computed(_lifetimeController.Lifetime, Adapt));
        }

        protected override void OnDestroy()
        {
            result.SetSource(null);
            _lifetimeController?.Dispose();

            base.OnDestroy();
        }

        private float Adapt()
        {
            return Mathf.Approximately(denominator.Value, 0f)
                ? 0f
                : numerator.Value / denominator.Value;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            numerator.Context.AddEditorListener(this);
            denominator.Context.AddEditorListener(this);

            result.SetContext(this);
            result.SetName(resultName);
        }

        public void OnEditorContextVariableChanged(ViewVariable variable)
        {
            if (variable.IsRootVariableFor(numerator) ||
                variable.IsRootVariableFor(denominator))
            {
                result.SetValueEditorOnly(Adapt());
                NotifyEditorVariableChanged(result);
            }
        }
#endif
    }
}