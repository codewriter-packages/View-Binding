using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
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