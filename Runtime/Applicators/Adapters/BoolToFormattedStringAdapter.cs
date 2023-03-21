using System;

namespace CodeWriter.ViewBinding.Applicators.Adapters {
    using UnityEngine;

    [AddComponentMenu("View Binding/Adapters/[Binding] Bool To Formatted String Adapter")]
    public class BoolToFormattedStringAdapter : SingleResultAdapterBase<string, ViewVariableString> {
        [Space]
        [SerializeField]
        private ViewVariableBool source;

        [SerializeField]
        private string trueFormat = "TRUE";

        [SerializeField]
        private string falseFormat = "FALSE";

        [SerializeField]
        private ViewContextBase[] extraContexts = Array.Empty<ViewContextBase>();

        protected override string Adapt() {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try {
                if (source.Value) {
                    textBuilder.AppendFormat(trueFormat, extraContexts);
                }
                else {
                    textBuilder.AppendFormat(falseFormat, extraContexts);
                }

                return new string(textBuilder.RawCharArray, 0, textBuilder.Length);
            }
            finally {
                textBuilder.Dispose();
            }
        }
    }
}