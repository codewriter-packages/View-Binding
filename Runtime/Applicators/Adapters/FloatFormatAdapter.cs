using System;
using TriInspector;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Float Format Adapter")]
    public class FloatFormatAdapter : SingleResultAdapterBase<float, FloatFormatAdapter.ViewVariableFloatFormatted>
    {
        [Space]
        [SerializeField]
        private ViewVariableFloat source = default;

        [ShowInInspector]
        public int Precision
        {
            get => result.precision;
            set => result.precision = value;
        }

        [ShowInInspector]
        public bool FixedPrecision
        {
            get => result.fixedPrecision;
            set => result.fixedPrecision = value;
        }

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

            public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
            {
                UnityEditor.EditorGUI.FloatField(position, label, Value, GUI.skin.label);
            }
#endif
        }

        protected override float Adapt()
        {
            return source.Value;
        }
    }
}