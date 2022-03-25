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
            var nameProp = property.FindPropertyRelative(NameFieldName);
            var contextProp = property.FindPropertyRelative(ContextFieldName);

            var nameRect = new Rect(position) {width = position.width / 2};
            var contextRect = new Rect(position) {xMin = nameRect.xMax};

            var entryDisplayName = nameProp.stringValue;
            var entryDisplayContent = new GUIContent(entryDisplayName, entryDisplayName);
            if (GUI.Button(nameRect, entryDisplayContent, EditorStyles.popup))
            {
                ShowEntryDropDown(nameRect, nameProp, contextProp);
            }

            var oldColor = GUI.color;

            GUI.color = oldColor * new Color(0.8f, 0.8f, 0.8f);

            var context = (ViewContextBase) contextProp.objectReferenceValue;
            var contextDisplayContent = new GUIContent(
                GetContextDisplayName(property, context, true),
                GetContextDisplayName(property, context));
            if (GUI.Button(contextRect, contextDisplayContent, EditorStyles.popup))
            {
                ShowContextDropDown(contextRect, property, contextProp);
            }

            GUI.color = oldColor;
        }

        protected abstract IEnumerable<TEntry> EnumerateEntries(ViewContextBase context);

        private void ShowContextDropDown(Rect position, SerializedProperty selfProperty, SerializedProperty contextProp)
        {
            if (selfProperty.serializedObject.targetObject is MonoBehaviour mb)
            {
                var matchedContexts = mb.GetComponentsInParent<ViewContextBase>();

                var menu = new GenericMenu();

                foreach (var matchedContext in matchedContexts)
                {
                    var isOn = contextProp.objectReferenceValue == matchedContext;
                    var name = GetContextDisplayName(selfProperty, matchedContext);
                    menu.AddItem(new GUIContent(name), isOn, SelectEntry, matchedContext);
                }

                menu.DropDown(position);

                void SelectEntry(object contextObject)
                {
                    contextProp.objectReferenceValue = (ViewContextBase) contextObject;
                    contextProp.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void ShowEntryDropDown(Rect position, SerializedProperty nameProp, SerializedProperty contextProp)
        {
            var context = (ViewContextBase) contextProp.objectReferenceValue;

            if (context == null)
            {
                return;
            }

            var matchedEntries = EnumerateEntries(context)
                .Where(o => o != null && o.GetType() == fieldInfo.FieldType)
                .ToList();

            var menu = new GenericMenu();

            foreach (var matchedEntry in matchedEntries)
            {
                var isOn = nameProp.stringValue == matchedEntry.Name;
                menu.AddItem(new GUIContent(matchedEntry.Name), isOn, SelectEntry, matchedEntry.Name);
            }

            menu.DropDown(position);

            void SelectEntry(object entryNameObject)
            {
                nameProp.stringValue = (string) entryNameObject;
                nameProp.serializedObject.ApplyModifiedProperties();
            }
        }

        private static string GetContextDisplayName(SerializedProperty selfProperty, ViewContextBase c,
            bool shortName = false)
        {
            if (c == null)
            {
                return "null";
            }

            if (shortName &&
                selfProperty.serializedObject.targetObject is MonoBehaviour mb &&
                mb.gameObject == c.gameObject)
            {
                return c.GetType().Name;
            }

            return $"{c.name} ({c.GetType().Name})";
        }
    }
}