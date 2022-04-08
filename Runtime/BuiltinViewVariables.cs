using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding
{
    [Serializable, Preserve, ExposedViewEntry]
    public sealed class ViewVariableBool : ViewVariable<bool, ViewVariableBool>
    {
        public override string TypeDisplayName => "Boolean";

        [Preserve]
        public ViewVariableBool()
        {
        }

        public override void AppendValueTo(ref ValueTextBuilder builder)
        {
            builder.Append(Value ? "yes" : "no");
        }

#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label,
            UnityEditor.SerializedProperty property, string variableName)
        {
            GUI.SetNextControlName(variableName);
            property.boolValue = UnityEditor.EditorGUI.Toggle(position, label, property.boolValue);
        }

        public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
        {
            UnityEditor.EditorGUI.Toggle(position, label, Value);
        }
#endif
    }

    [Serializable, Preserve, ExposedViewEntry]
    public sealed class ViewVariableInt : ViewVariable<int, ViewVariableInt>
    {
        public override string TypeDisplayName => "Integer";

        [Preserve]
        public ViewVariableInt()
        {
        }

        public override void AppendValueTo(ref ValueTextBuilder builder)
        {
            builder.Append(Value);
        }

#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label,
            UnityEditor.SerializedProperty property, string variableName)
        {
            GUI.SetNextControlName(variableName);
            UnityEditor.EditorGUI.DelayedIntField(position, property, label);
        }

        public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
        {
            UnityEditor.EditorGUI.IntField(position, label, Value, GUI.skin.label);
        }
#endif
    }

    [Serializable, Preserve, ExposedViewEntry]
    public sealed class ViewVariableFloat : ViewVariable<float, ViewVariableFloat>
    {
        public override string TypeDisplayName => "Float";

        [Preserve]
        public ViewVariableFloat()
        {
        }

        public override void AppendValueTo(ref ValueTextBuilder builder)
        {
            builder.Append(Value);
        }

#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label,
            UnityEditor.SerializedProperty property, string variableName)
        {
            GUI.SetNextControlName(variableName);
            property.floatValue = UnityEditor.EditorGUI.FloatField(position, label, property.floatValue);
        }

        public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
        {
            UnityEditor.EditorGUI.FloatField(position, label, Value, GUI.skin.label);
        }
#endif
    }

    [Serializable, Preserve, ExposedViewEntry]
    public sealed class ViewVariableString : ViewVariable<string, ViewVariableString>
    {
        public override string TypeDisplayName => "String";

        [Preserve]
        public ViewVariableString()
        {
        }

        public override void AppendValueTo(ref ValueTextBuilder builder)
        {
            builder.Append(Value);
        }

#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label,
            UnityEditor.SerializedProperty property, string variableName)
        {
            if (EnumVariableUtils.TryGetEnumValues(variableName, out var enumValues))
            {
                GUI.SetNextControlName(variableName);

                var selected = Array.IndexOf(enumValues, property.stringValue);
                var newSelected = UnityEditor.EditorGUI.Popup(position, label.text, selected, enumValues);
                if (newSelected != selected && newSelected != -1)
                {
                    property.stringValue = enumValues[newSelected];
                }
            }
            else
            {
                GUI.SetNextControlName(variableName);
                UnityEditor.EditorGUI.DelayedTextField(position, property, label);
            }
        }

        public override void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
        {
            UnityEditor.EditorGUI.LabelField(position, label.text, Value);
        }
#endif
    }
}