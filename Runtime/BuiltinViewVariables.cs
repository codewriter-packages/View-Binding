using System;
using System.Text;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public sealed class ViewVariableBool : ViewVariable<bool, ViewVariableBool>
    {
        public override string TypeDisplayName => "Boolean";

        public override void AppendValueTo(ref StringBuilder builder)
        {
            builder.Append(Value ? "yes" : "no");
        }
        
#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label, 
            UnityEditor.SerializedProperty property, string variableName)
        {
            property.boolValue = UnityEditor.EditorGUI.Toggle(position, label, property.boolValue);
        }
#endif
    }

    [Serializable]
    public sealed class ViewVariableInt : ViewVariable<int, ViewVariableInt>
    {
        public override string TypeDisplayName => "Integer";

        public override void AppendValueTo(ref StringBuilder builder)
        {
            NonAllocFormatter.AppendInvariant(builder, Value);
        }
        
#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label, 
            UnityEditor.SerializedProperty property, string variableName)
        {
            UnityEditor.EditorGUI.DelayedIntField(position, property, label);
        }
#endif
    }

    [Serializable]
    public sealed class ViewVariableFloat : ViewVariable<float, ViewVariableFloat>
    {
        public override string TypeDisplayName => "Float";

        public override void AppendValueTo(ref StringBuilder builder) => builder.Append(Value);
        
#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label, 
            UnityEditor.SerializedProperty property, string variableName)
        {
            property.floatValue = UnityEditor.EditorGUI.FloatField(position, label, property.floatValue);
        }
#endif
    }

    [Serializable]
    public sealed class ViewVariableString : ViewVariable<string, ViewVariableString>
    {
        public override string TypeDisplayName => "String";
        public override void AppendValueTo(ref StringBuilder builder) => builder.Append(Value);
        
#if UNITY_EDITOR
        public override void DoGUI(Rect position, GUIContent label, 
            UnityEditor.SerializedProperty property, string variableName)
        {
            if (EnumVariableUtils.TryGetEnumValues(variableName, out var enumValues))
            {
                var selected = Array.IndexOf(enumValues, property.stringValue);
                var newSelected = UnityEditor.EditorGUI.Popup(position, label.text, selected, enumValues);
                if (newSelected != selected && newSelected != -1)
                {
                    property.stringValue = enumValues[newSelected];
                }
            }
            else
            {
                UnityEditor.EditorGUI.DelayedTextField(position, property, label);
            }
        }
#endif
    }
}