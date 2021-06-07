using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomPropertyDrawer(typeof(ViewVariable), true)]
    internal class ViewVariableDrawer : PropertyDrawer
    {
        private const string NameFieldName = "name";
        private const string ContextFieldName = "context";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                DrawContent(position, property);
            }

            EditorGUI.EndProperty();
        }

        private void DrawContent(Rect position, SerializedProperty property)
        {
            var contextProp = property.FindPropertyRelative(ContextFieldName);
            var nameProp = property.FindPropertyRelative(NameFieldName);

            if (contextProp.objectReferenceValue == null)
            {
                var contextRect = new Rect(position) {width = position.width / 2};
                var findRect = new Rect(position) {xMin = contextRect.xMax};

                EditorGUI.PropertyField(contextRect, contextProp, GUIContent.none);

                if (property.serializedObject.targetObject is MonoBehaviour mb &&
                    GUI.Button(findRect, "Find Context in Parents"))
                {
                    var parentContext = mb.GetComponentInParent<ViewContextBase>();
                    if (parentContext == mb)
                    {
                        parentContext = null;

                        if (mb.transform.parent != null)
                        {
                            parentContext = mb.transform.parent.GetComponentInParent<ViewContextBase>();
                        }
                    }

                    contextProp.objectReferenceValue = parentContext;
                    contextProp.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                var contextRect = new Rect(position) {width = position.width / 2};
                var variablesRect = new Rect(position) {xMin = contextRect.xMax};

                EditorGUI.PropertyField(contextRect, contextProp, GUIContent.none);

                var context = (ViewContextBase) contextProp.objectReferenceValue;

                var matchedVariables = Enumerable.Range(0, context.VariablesCount)
                    .Select(index => context.GetVariable(index))
                    .Where(o => o != null && o.GetType() == fieldInfo.FieldType)
                    .ToList();

                var oldSelectedIndex = GetMatchedVariableIndex(matchedVariables, nameProp.stringValue) + 1;

                var popupOptions = matchedVariables
                    .Select(o => new GUIContent(o.Name))
                    .Prepend(new GUIContent("NONE"))
                    .ToArray();

                var newSelectedIndex = EditorGUI.Popup(variablesRect, oldSelectedIndex, popupOptions);

                if (newSelectedIndex != oldSelectedIndex && newSelectedIndex != -1)
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "Change Linked Variable");

                    nameProp.stringValue = newSelectedIndex > 0
                        ? matchedVariables[newSelectedIndex - 1].Name
                        : string.Empty;

                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
        }

        private static int GetMatchedVariableIndex(List<ViewVariable> variables, string name)
        {
            for (var index = 0; index < variables.Count; index++)
            {
                var variable = variables[index];
                if (variable.Name == name)
                {
                    return index;
                }
            }

            return -1;
        }
    }
}