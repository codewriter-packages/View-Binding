using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Bool to String Adapter")]
    public class BoolToStringAdapter : SingleResultAdapterBase<string, ViewVariableString>
    {
        [Space]
        [SerializeField]
        private ViewVariableBool source;

        [SerializeField]
        private string trueString = "TRUE";

        [SerializeField]
        private string falseString = "FALSE";

        protected override string Adapt()
        {
            return source.Value ? trueString : falseString;
        }
    }
}