using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/Bool Inverse")]
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