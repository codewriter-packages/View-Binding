using System.Text;
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

        private StringBuilder _stringBuilder;

        protected override string Adapt()
        {
            if (_stringBuilder == null)
            {
                _stringBuilder = new StringBuilder();
            }

            TextFormatUtility.FormatText(_stringBuilder, format, null, contexts);

            return BindingsLocalization.Localize(_stringBuilder);
        }
    }
}