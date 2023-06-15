using System;
using TriInspector;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Bool To Formatted String Adapter")]
    public class BoolToFormattedStringAdapter : SingleResultAdapterBase<string, ViewVariableString>
    {
        [Space]
        [SerializeField]
        private ViewVariableBool source;

        [Required]
        [SerializeField]
        private string trueFormat = "TRUE";

        [Required]
        [SerializeField]
        private string falseFormat = "FALSE";

        [Required]
        [SerializeField]
        [ViewContextCollection]
        private ViewContextBase[] extraContexts = Array.Empty<ViewContextBase>();

        protected override string Adapt()
        {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                textBuilder.AppendFormat(source.Value ? trueFormat : falseFormat, extraContexts);

                return new string(textBuilder.RawCharArray, 0, textBuilder.Length);
            }
            finally
            {
                textBuilder.Dispose();
            }
        }
    }
}