using System;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/Float Format")]
    public class FloatFormatAdapter : SingleResultAdapterBase<float, FloatFormatAdapter.ViewVariableFloatFormatted>
    {
        [Space]
        [SerializeField]
        private ViewVariableFloat source = default;

        [Serializable]
        public class ViewVariableFloatFormatted : ViewVariable<float, ViewVariableFloatFormatted>
        {
            public override string TypeDisplayName => "Float (Formatted)";

            public int precision = 1;
            public bool fixedPrecision = true;

            public override void AppendValueTo(ref ValueTextBuilder builder)
            {
                builder.Append(Value, precision, fixedPrecision);
            }

#if UNITY_EDITOR
            public override void DoGUI(Rect position, GUIContent label,
                UnityEditor.SerializedProperty property, string variableName)
            {
                property.floatValue = UnityEditor.EditorGUI.FloatField(position, label, property.floatValue);
            }
#endif
        }

        protected override float Adapt()
        {
            return source.Value;
        }
    }
}