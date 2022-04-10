using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/Text Localize")]
    public class TextLocalizeAdapter : SingleResultAdapterBase<string, TextLocalizeAdapter.ViewVariableStringLocalized>
    {
        [Space]
        [SerializeField]
        private string format = "";

        protected override string Adapt()
        {
            return format;
        }

        [Serializable, Preserve]
        public class ViewVariableStringLocalized : ViewVariable<string, ViewVariableStringLocalized>
        {
            [SerializeField]
            private ViewContextBase[] extraContexts = null;

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

#if UNITY_EDITOR
            public override void DoGUI(Rect position, GUIContent label, SerializedProperty property,
                string variableName)
            {
            }

            public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
            {
            }
#endif
        }
    }
}