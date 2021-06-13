using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomEditor(typeof(ViewContext), true)]
    internal class ViewContextEditor : UnityEditor.Editor
    {
        private const string VariablesFieldName = "vars";
        private const string EventsFieldName = "evts";
        private const string NameFieldName = "name";
        private const string ContextFieldName = "context";
        private const string ValueFieldName = "value";

        private SerializedProperty _variablesProp;
        private ReorderableList _variablesListDrawer;
        private FieldInfo _variablesFieldInfo;

        private SerializedProperty _eventsProp;
        private ReorderableList _eventsListDrawer;
        private FieldInfo _eventsFieldInfo;

        private ViewContextBase TargetContext => (ViewContextBase) target;

        private void OnEnable()
        {
            _variablesProp = serializedObject.FindProperty(VariablesFieldName);
            _variablesFieldInfo =
                ScriptAttributeUtilityProxy.GetFieldInfoAndStaticTypeFromProperty(_variablesProp, out _);
            _variablesListDrawer =
                CreateEntryList<ViewVariable>(_variablesProp, "Variables", DrawVariable, TargetContext);

            _eventsProp = serializedObject.FindProperty(EventsFieldName);
            _eventsFieldInfo = ScriptAttributeUtilityProxy.GetFieldInfoAndStaticTypeFromProperty(_eventsProp, out _);
            _eventsListDrawer = CreateEntryList<ViewEvent>(_eventsProp, "Events", DrawEvent, TargetContext);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                _variablesListDrawer.DoLayoutList();
                _eventsListDrawer.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private T GetListElement<T>(FieldInfo fi, int index) => ((List<T>) fi.GetValue(target))[index];

        private void DrawVariable(Rect rect, int index, bool active, bool elementFocused)
        {
            var variableProp = _variablesProp.GetArrayElementAtIndex(index);
            var contextProp = variableProp.FindPropertyRelative(ContextFieldName);
            var nameProp = variableProp.FindPropertyRelative(NameFieldName);
            var valueProp = variableProp.FindPropertyRelative(ValueFieldName);

            var variableInstance = GetListElement<ViewVariable>(_variablesFieldInfo, index);
            if (variableInstance == null || contextProp.objectReferenceValue == null)
            {
                return;
            }

            var nameRect = new Rect(rect) {width = (rect.width - 135) / 2};
            var valueRect = new Rect(rect) {xMin = nameRect.xMax};

            DrawName(nameRect, nameProp, "CW_VB_variable_name_", index, elementFocused);

            EditorGUI.BeginChangeCheck();

            var valueContent = new GUIContent(variableInstance.TypeDisplayName);

            var oldLabelWidth = EditorGUIUtility.labelWidth;
            var oldIntentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = 80;
            EditorGUI.indentLevel = 0;

            if (Application.isPlaying)
            {
                EditorGUI.LabelField(valueRect, valueContent, GUIContent.none);
            }
            else
            {
                if (valueProp.propertyType == SerializedPropertyType.String)
                {
                    EditorGUI.DelayedTextField(valueRect, valueProp, valueContent);
                }
                else
                {
                    EditorGUI.PropertyField(valueRect, valueProp, valueContent);
                }
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIntentLevel;

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                TargetContext.NotifyEditorVariableChanged(variableInstance);
            }
        }

        private void DrawEvent(Rect rect, int index, bool active, bool elementFocused)
        {
            var eventProp = _eventsProp.GetArrayElementAtIndex(index);
            var contextProp = eventProp.FindPropertyRelative(ContextFieldName);
            var nameProp = eventProp.FindPropertyRelative(NameFieldName);

            var eventInstance = GetListElement<ViewEvent>(_eventsFieldInfo, index);
            if (eventInstance == null || contextProp.objectReferenceValue == null)
            {
                return;
            }

            var nameRect = new Rect(rect) {width = (rect.width - 135) / 2};
            var valueRect = new Rect(rect) {xMin = nameRect.xMax};

            DrawName(nameRect, nameProp, "CW_VB_event_name_", index, elementFocused);

            var valueContent = new GUIContent(eventInstance.TypeDisplayName);

            var oldLabelWidth = EditorGUIUtility.labelWidth;
            var oldIntentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = 80;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(valueRect, valueContent, GUIContent.none);

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIntentLevel;
        }

        private static void DrawName(Rect nameRect, SerializedProperty nameProp,
            string key, int index, bool elementFocused)
        {
            var focusedControlName = GUI.GetNameOfFocusedControl();
            var nameFocused = focusedControlName.StartsWith(key) && focusedControlName == key + index;

            if (elementFocused || nameFocused)
            {
                var nameStyle = nameFocused ? EditorStyles.textField : EditorStyles.label;

                GUI.SetNextControlName(key + index);
                var newName = GUI.TextField(nameRect, nameProp.stringValue, nameStyle);
                if (newName != nameProp.stringValue)
                {
                    nameProp.stringValue = newName;
                }

                var isEnterPressed = Event.current.isKey && Event.current.keyCode == KeyCode.Return;
                if (isEnterPressed)
                {
                    Event.current.Use();
                    GUI.FocusControl(null);
                }
            }
            else if (!string.IsNullOrWhiteSpace(nameProp.stringValue))
            {
                GUI.Label(nameRect, nameProp.stringValue);
            }
            else
            {
                GUI.Label(nameRect, "Unnamed", Styles.RedBoldLabel);
            }
        }

        private static ReorderableList CreateEntryList<TBase>(SerializedProperty prop, string header,
            ReorderableList.ElementCallbackDelegate drawElement, ViewContextBase context)
            where TBase : ViewEntry
        {
            return new ReorderableList(prop.serializedObject, prop)
            {
                drawHeaderCallback = rect => GUI.Label(rect, header),
                elementHeightCallback = index => EditorGUIUtility.singleLineHeight,
                drawElementCallback = drawElement,
                onAddDropdownCallback = (rect, list) => ShowEntryDropdown<TBase>(prop, context),
            };
        }

        private static void ShowEntryDropdown<TBase>(SerializedProperty arrayProp, ViewContextBase context)
            where TBase : ViewEntry
        {
            var menu = new GenericMenu();

            foreach (var type in EnumerateTypes<TBase>())
            {
                var instance = (ViewEntry) Activator.CreateInstance(type);

                menu.AddItem(new GUIContent(instance.TypeDisplayName), false, () =>
                {
                    instance.SetContext(context);

                    var index = arrayProp.arraySize;
                    arrayProp.InsertArrayElementAtIndex(index);
                    arrayProp.GetArrayElementAtIndex(index).managedReferenceValue = instance;
                    arrayProp.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }

        private static IEnumerable<Type> EnumerateTypes<TBase>()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => typeof(TBase).IsAssignableFrom(type) && !type.IsAbstract);
        }
    }
}