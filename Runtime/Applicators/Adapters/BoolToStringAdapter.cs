using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
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

        protected override bool IsVariableUsed(ViewVariable variable)
        {
            return variable.IsRootVariableFor(source);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (source != null && source.Context != null)
            {
                source.Context.AddEditorListener(this);
            }
        }
#endif
    }
}