using TriInspector;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    [AddComponentMenu("View Binding/Adapters/[Binding] Formatted Text Adapter")]
    public class FormattedTextAdapter : SingleResultAdapterBase<string, FormattedTextAdapter.ViewVariableString>
    {
        [Space]
        [SerializeField]
        private string format = "";

        [Required]
        [ShowInInspector]
        [ViewContextCollection]
        public ViewContextBase[] ExtraContexts
        {
            get => result.extraContexts;
            set => result.extraContexts = value;
        }

        protected override string Adapt()
        {
            return format;
        }

        [Serializable, Preserve]
        public class ViewVariableString : ViewVariable<string, ViewVariableString>
        {
            [SerializeField]
            public ViewContextBase[] extraContexts = null;

            [Preserve]
            public ViewVariableString()
            {
            }

            public override void AppendValueTo(ref ValueTextBuilder builder)
            {
                var formatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
                var secondFormatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
                try
                {
                    formatTextBuilder.AppendFormat(Value, extraContexts);
                    secondFormatTextBuilder.AppendFormat(formatTextBuilder.ToString(), extraContexts);

                    builder.Append(secondFormatTextBuilder.AsSpan());
                }
                finally
                {
                    formatTextBuilder.Dispose();
                }
            }
        }
    }
}