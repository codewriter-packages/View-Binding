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
        private const string ListenersFieldName = "listeners";
        private const string VariablesFieldName = "vars";
        private const string EventsFieldName = "evts";
        private const string NameFieldName = "name";
        private const string ContextFieldName = "context";
        private const string ValueFieldName = "value";

        private static readonly string[] ExcludedProps =
        {
            "m_Script",
            ListenersFieldName,
            VariablesFieldName,
            EventsFieldName
        };

        private SerializedProperty _listenersProp;

        private SerializedProperty _variablesProp;
        private ReorderableList _variablesListDrawer;
        private FieldInfo _variablesFieldInfo;

        private SerializedProperty _eventsProp;
        private ReorderableList _eventsListDrawer;
        private FieldInfo _eventsFieldInfo;

        private ViewContext TargetContext => (ViewContext) target;

        private void OnEnable()
        {
            _listenersProp = serializedObject.FindProperty(ListenersFieldName);

            _variablesProp = serializedObject.FindProperty(VariablesFieldName);
            _variablesFieldInfo =
                ScriptAttributeUtilityProxy.GetFieldInfoAndStaticTypeFromProperty(_variablesProp, out _);
            _variablesListDrawer =
                CreateEntryList<ViewVariable>(_variablesProp, "Variables", DrawVariable, TargetContext);

            _eventsProp = serializedObject.FindProperty(EventsFieldName);
            _eventsFieldInfo = ScriptAttributeUtilityProxy.GetFieldInfoAndStaticTypeFromProperty(_eventsProp, out _);
            _eventsListDrawer = CreateEntryList<ViewEvent>(_eventsProp, "Events", DrawEvent, TargetContext);

            _variablesListDrawer.drawHeaderCallback = DoVariablesHeader;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var disabled = Application.isPlaying ||
                           (EditorUtility.IsPersistent(serializedObject.targetObject));
            using (new EditorGUI.DisabledScope(disabled))
            {
                DoListenersGUI();

                _variablesListDrawer.DoLayoutList();
                _eventsListDrawer.DoLayoutList();
                
                DrawPropertiesExcluding(serializedObject, ExcludedProps);
            }

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            {
                _variablesListDrawer.ReleaseKeyboardFocus();
                _eventsListDrawer.ReleaseKeyboardFocus();
                _variablesListDrawer.index = -1;
                _eventsListDrawer.index = -1;
                Repaint();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DoVariablesHeader(Rect rect)
        {
            var labelRect = new Rect(rect) {xMax = rect.xMax - 80};
            var applyRect = new Rect(rect) {xMin = labelRect.xMax};

            GUI.Label(labelRect, "Variables");
            if (GUI.Button(applyRect, "Apply"))
            {
                ApplyApplicators();
            }
        }

        private void DoListenersGUI()
        {
            var headerRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.label);
            var headerLabelRect = new Rect(headerRect)
            {
                width = EditorGUIUtility.labelWidth
            };
            var headerContentRect = new Rect(headerRect)
            {
                xMin = headerLabelRect.xMax
            };

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.PropertyField(headerLabelRect, _listenersProp, false);

                if (_listenersProp.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    for (int i = 0, len = _listenersProp.arraySize; i < len; i++)
                    {
                        EditorGUILayout.PropertyField(_listenersProp.GetArrayElementAtIndex(i));
                    }

                    if (_listenersProp.arraySize == 0)
                    {
                        GUILayout.Label("No listeners");
                    }

                    EditorGUI.indentLevel--;
                }
            }

            if (GUI.Button(headerContentRect, "Fill Listeners"))
            {
                FillListeners();
            }

            EditorGUILayout.Space();
        }

        private void FillListeners()
        {
            serializedObject.ApplyModifiedProperties();

            var viewContext = (ViewContext) target;

            viewContext.FillListeners();

            EditorUtility.SetDirty(viewContext);
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
                if (valueProp == null)
                {
                    EditorGUI.LabelField(valueRect, GUIContent.none, valueContent);
                }
                else
                {
                    variableInstance.DoGUI(valueRect, valueContent, valueProp, nameProp.stringValue);
                }
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIntentLevel;

            if (EditorGUI.EndChangeCheck())
            {
                ApplyApplicators();
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

            foreach (var type in EnumerateViewEntryTypes<TBase>())
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

        private void ApplyApplicators()
        {
            serializedObject.ApplyModifiedProperties();

            TargetContext.FillListeners();

            foreach (var listener in TargetContext.Listeners)
            {
                if (listener is ApplicatorBase applicator)
                {
                    applicator.Apply();
                }
            }
        }

        private static IEnumerable<Type> EnumerateViewEntryTypes<TBase>()
            where TBase : ViewEntry
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => typeof(TBase).IsAssignableFrom(type) && !type.IsAbstract)
                .OrderBy(type => type.Name);
        }
    }
}