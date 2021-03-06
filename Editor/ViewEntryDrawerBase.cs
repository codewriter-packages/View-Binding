using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    internal abstract class ViewEntryDrawerBase<TEntry> : PropertyDrawer
        where TEntry : ViewEntry
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

            var disabled = Application.isPlaying ||
                           (EditorUtility.IsPersistent(property.serializedObject.targetObject));
            using (new EditorGUI.DisabledScope(disabled))
            {
                DrawContent(position, property);
            }

            EditorGUI.EndProperty();
        }

        private void DrawContent(Rect position, SerializedProperty property)
        {
            var nameProp = property.FindPropertyRelative(NameFieldName);
            var contextProp = property.FindPropertyRelative(ContextFieldName);

            if (GUI.Button(position, nameProp.stringValue, EditorStyles.popup))
            {
                ShowEntryDropDown(position, property, nameProp, contextProp);
            }
        }

        protected abstract IEnumerable<TEntry> EnumerateEntries(ViewContextBase context);

        private void ShowEntryDropDown(Rect position, SerializedProperty selfProp, SerializedProperty nameProp,
            SerializedProperty contextProp)
        {
            var menu = new GenericMenu();

            if (selfProp.serializedObject.targetObject is MonoBehaviour mb)
            {
                var matchedContexts = Enumerable.Empty<ViewContextBase>()
                    .Concat(mb.GetComponentsInParent<ViewContextBase>(true))
                    .Where(it => it != null && it != mb)
                    .ToList();

                for (var contextIndex = 0; contextIndex < matchedContexts.Count; contextIndex++)
                {
                    var matchedContext = matchedContexts[contextIndex];
                    var matchedEntries = EnumerateEntries(matchedContext)
                        .Where(o => o != null && o.GetType() == fieldInfo.FieldType)
                        .ToList();

                    foreach (var matchedEntry in matchedEntries)
                    {
                        var isOn = nameProp.stringValue == matchedEntry.Name;
                        var name = $"{matchedEntry.Name} - {matchedContext.name} ({matchedContext.GetType().Name})";
                        menu.AddItem(new GUIContent(name), isOn, () =>
                        {
                            nameProp.stringValue = matchedEntry.Name;
                            contextProp.objectReferenceValue = matchedContext;
                            nameProp.serializedObject.ApplyModifiedProperties();
                        });
                    }
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Target object must be MonoBehaviour"));
            }

            menu.DropDown(position);
        }
    }
}