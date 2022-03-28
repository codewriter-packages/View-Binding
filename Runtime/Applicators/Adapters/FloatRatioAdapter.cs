using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
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

        protected override bool IsVariableUsed(ViewVariable variable)
        {
            return variable.IsRootVariableFor(numerator) || variable.IsRootVariableFor(denominator);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.EditorTrackModificationsOf(numerator);
            this.EditorTrackModificationsOf(denominator);
        }
#endif
    }
}