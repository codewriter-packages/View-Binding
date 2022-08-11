using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Bool Inverse Adapter")]
    public class InverseBoolAdapter : SingleResultAdapterBase<bool, ViewVariableBool>
    {
        [Space]
        [SerializeField]
        private ViewVariableBool source;

        protected override bool Adapt()
        {
            return !source.Value;
        }
    }
}