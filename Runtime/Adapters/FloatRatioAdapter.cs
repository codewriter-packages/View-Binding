using UnityEngine;

namespace CodeWriter.ViewBinding.Adapters
{
    public sealed class FloatRatioAdapter : AdapterBase
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

        internal override int VariablesCount => 1;

        internal override ViewVariable GetVariable(int index) => result;

        protected override void OnValidate()
        {
            result.SetContext(this);
            result.SetName(resultName);

            base.OnValidate();

            Apply();
        }

        protected override void ReSubscribe()
        {
            numerator.Subscribe(this);
            denominator.Subscribe(this);
        }

        public override void OnContextVariableChanged(ViewVariable variable)
        {
            if (numerator.IsLinkedTo(variable) ||
                denominator.IsLinkedTo(variable))
            {
                Apply();
            }
        }

        private void Apply()
        {
            result.Value = Mathf.Approximately(denominator.Value, 0f)
                ? 0f
                : numerator.Value / denominator.Value;

            OnVariableChanged(result);
        }
    }
}