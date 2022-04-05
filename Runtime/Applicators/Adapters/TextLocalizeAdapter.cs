using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public class TextLocalizeAdapter : SingleResultAdapterBase<string, ViewVariableString>
    {
        [Space]
        [SerializeField]
        private string format = "";

        [SerializeField]
        private ViewContextBase[] contexts = null;

        protected override string Adapt()
        {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                TextFormatUtility.FormatText(ref textBuilder, format, null, contexts);
                return BindingsLocalization.Localize(ref textBuilder);
            }
            finally
            {
                textBuilder.Dispose();
            }
        }
    }
}