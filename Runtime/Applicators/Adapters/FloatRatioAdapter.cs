using UnityEngine;
using UnityEngine.Serialization;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public sealed class FloatRatioAdapter : ViewContextBase
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        [SerializeField]
        [FormerlySerializedAs("resultName")]
        private string alias = "Result";

        [Space]
        [SerializeField]
        private ViewVariableFloat numerator;

        [SerializeField]
        private ViewVariableFloat denominator;
        
        [SerializeField]
        [HideInInspector]
        private ViewVariableFloat result;

        public bool IsDestroyed => this == null;

        protected override int VariablesCount => 1;
        protected override int EventCount => 0;

        protected override ViewVariable GetVariable(int index) => result;
        protected override ViewEvent GetEvent(int index) => null;

        protected override void Start()
        {
            base.Start();

            result.SetSource(Adapt);
        }

        protected override void OnDestroy()
        {
            result.SetSource(null);

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

            if (numerator != null && numerator.Context != null)
            {
                numerator.Context.AddEditorListener(this);
            }

            if (denominator != null && denominator.Context != null)
            {
                denominator.Context.AddEditorListener(this);
            }

            result.SetContext(this);
            result.SetName(alias);
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