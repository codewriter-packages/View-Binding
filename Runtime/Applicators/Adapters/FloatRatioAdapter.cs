using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/Float Ratio")]
    public sealed class FloatRatioAdapter : SingleResultAdapterBase<float, ViewVariableFloat>
    {
        [Space]
        [SerializeField]
        private ViewVariableFloat numerator;

        [SerializeField]
        private ViewVariableFloat denominator;

        protected override float Adapt()
        {
            return Mathf.Approximately(denominator.Value, 0f)
                ? 0f
                : numerator.Value / denominator.Value;
        }
    }
}