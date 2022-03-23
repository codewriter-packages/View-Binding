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
                var oldColor = GUI.color;

                var contextRect = new Rect(position) {width = position.width / 2};
                var findRect = new Rect(position) {xMin = contextRect.xMax};

                GUI.color = oldColor * new Color(1f, 0.7f, 0.7f);
                EditorGUI.PropertyField(contextRect, contextProp, GUIContent.none);

                GUI.color = oldColor * new Color(0.9f, 1f, 1f);
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

                GUI.color = oldColor;
            }
            else
            {
                var contextRect = new Rect(position) {width = position.width / 2};
                var dropdownRect = new Rect(position) {xMin = contextRect.xMax};

                EditorGUI.PropertyField(contextRect, contextProp, GUIContent.none);

                var context = (ViewContextBase) contextProp.objectReferenceValue;

                var matchedEntries = EnumerateEntries(context)
                    .Where(o => o != null && o.GetType() == fieldInfo.FieldType)
                    .ToList();

                var oldSelectedIndex = GetMatchedEntryIndex(matchedEntries, nameProp.stringValue) + 1;

                var popupOptions = matchedEntries
                    .Select(o => new GUIContent(o.Name))
                    .Prepend(new GUIContent("NONE"))
                    .ToArray();

                var oldColor = GUI.color;
                if (oldSelectedIndex == 0)
                {
                    GUI.color = oldColor * new Color(1f, 0.7f, 0.7f);
                }

                var newSelectedIndex = EditorGUI.Popup(dropdownRect, oldSelectedIndex, popupOptions);

                GUI.color = oldColor;

                if (newSelectedIndex != oldSelectedIndex && newSelectedIndex != -1)
                {
                    nameProp.stringValue = newSelectedIndex > 0
                        ? matchedEntries[newSelectedIndex - 1].Name
                        : string.Empty;

                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        protected abstract IEnumerable<TEntry> EnumerateEntries(ViewContextBase context);

        private static int GetMatchedEntryIndex(List<TEntry> entries, string name)
        {
            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];
                if (entry.Name == name)
                {
                    return index;
                }
            }

            return -1;
        }
    }
}