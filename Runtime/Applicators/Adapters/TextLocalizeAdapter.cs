using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Text Localize Adapter")]
    public class TextLocalizeAdapter : SingleResultAdapterBase<string, TextLocalizeAdapter.ViewVariableStringLocalized>
    {
        [Space]
        [Required]
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
        public class ViewVariableStringLocalized : ViewVariable<string, ViewVariableStringLocalized>
        {
            [SerializeField]
            internal ViewContextBase[] extraContexts = null;

            [Preserve]
            public ViewVariableStringLocalized()
            {
            }

            public override void AppendValueTo(ref ValueTextBuilder builder)
            {
                var formatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
                var localizedTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
                try
                {
                    formatTextBuilder.AppendFormat(Value, extraContexts);
                    var localizedString = BindingsLocalization.Localize(ref formatTextBuilder);

                    localizedTextBuilder.AppendFormat(localizedString, extraContexts);

                    builder.Append(localizedTextBuilder.AsSpan());
                }
                finally
                {
                    formatTextBuilder.Dispose();
                    localizedTextBuilder.Dispose();
                }
            }
        }
    }
}