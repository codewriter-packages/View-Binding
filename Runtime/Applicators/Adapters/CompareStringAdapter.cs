using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] String Compare Adapter")]
    public class CompareStringAdapter : SingleResultAdapterBase<bool, ViewVariableBool>
    {
        [Space]
        [SerializeField]
        private ViewVariableString source;

        [SerializeField]
        private CompareType comparer = CompareType.Equals;

        [SerializeField]
        private string other = "";

        protected override bool Adapt()
        {
            switch (comparer)
            {
                case CompareType.Equals:
                    return source.Value == other;

                case CompareType.NotEquals:
                    return source.Value != other;

                default:
                    return false;
            }
        }

        private enum CompareType
        {
            Equals,
            NotEquals,
        }
    }
}