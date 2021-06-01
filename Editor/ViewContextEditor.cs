using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomEditor(typeof(ViewContext))]
    internal class ViewContextEditor : UnityEditor.Editor
    {
        private static readonly GUIContent VariablesHeaderContent = new GUIContent("Variables");

        private const string VariablesFieldName = "variables";
        private const string NameFieldName = "name";
        private const string ContextFieldName = "context";
        private const string ValueFieldName = "value";

        private SerializedProperty _elementsProp;
        private ReorderableList _variablesListDrawer;
        private FieldInfo _variablesFieldInfo;

        private ViewContextBase TargetContext => (ViewContextBase) target;

        private void OnEnable()
        {
            _elementsProp = serializedObject.FindProperty(VariablesFieldName);
            _variablesFieldInfo =
                ScriptAttributeUtilityProxy.GetFieldInfoAndStaticTypeFromProperty(_elementsProp, out _);
            _variablesListDrawer = CreateVariablesList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _variablesListDrawer.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private ReorderableList CreateVariablesList()
        {
            return new ReorderableList(serializedObject, _elementsProp)
            {
                drawHeaderCallback = rect => GUI.Label(rect, VariablesHeaderContent),
                elementHeightCallback = index => EditorGUIUtility.singleLineHeight,
                drawElementCallback = DrawVariable,
                onAddDropdownCallback = (rect, list) => ShowAddVariableDropdown(),
            };
        }

        private void DrawVariable(Rect rect, int index, bool active, bool elementFocused)
        {
            var elementProp = _elementsProp.GetArrayElementAtIndex(index);
            var contextProp = elementProp.FindPropertyRelative(ContextFieldName);
            var nameProp = elementProp.FindPropertyRelative(NameFieldName);
            var valueProp = elementProp.FindPropertyRelative(ValueFieldName);

            var variableInstance = GetVariable(index);
            if (variableInstance == null)
            {
                return;
            }

            if (contextProp.objectReferenceValue == null)
            {
                if (GUI.Button(rect, "Link"))
                {
                    contextProp.objectReferenceValue = target;
                    serializedObject.ApplyModifiedProperties();
                }

                return;
            }

            var nameRect = new Rect(rect) {width = (rect.width - 135) / 2};
            var valueRect = new Rect(rect) {xMin = nameRect.xMax};

            var nameFocused = GUI.GetNameOfFocusedControl().StartsWith("CW_VB_variable_name_") &&
                              GUI.GetNameOfFocusedControl().EndsWith(index.ToString());

            if (elementFocused || nameFocused)
            {
                var nameStyle = nameFocused ? EditorStyles.textField : EditorStyles.label;

                GUI.SetNextControlName("CW_VB_variable_name_" + index);
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

            EditorGUI.BeginChangeCheck();

            var oldLabelWidth = EditorGUIUtility.labelWidth;
            var oldIntentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = 80;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(valueRect, valueProp, new GUIContent(variableInstance.TypeDisplayName));
            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIntentLevel;

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                TargetContext.OnVariableChanged(variableInstance);
            }
        }

        private void ShowAddVariableDropdown()
        {
            var menu = new GenericMenu();

            foreach (var variableType in EnumerateViewVariableTypes())
            {
                var variableInstance = (ViewVariable) Activator.CreateInstance(variableType);

                menu.AddItem(new GUIContent(variableInstance.TypeDisplayName), false, () =>
                {
                    variableInstance.SetContext(TargetContext);

                    Undo.RecordObject(target, "Add View Variable");
                    var index = _elementsProp.arraySize;
                    _elementsProp.InsertArrayElementAtIndex(index);
                    _elementsProp.GetArrayElementAtIndex(index).managedReferenceValue = variableInstance;
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                });
            }

            menu.ShowAsContext();
        }

        private ViewVariable GetVariable(int index)
        {
            return ((List<ViewVariable>) _variablesFieldInfo.GetValue(target))[index];
        }

        private static IEnumerable<Type> EnumerateViewVariableTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => typeof(ViewVariable).IsAssignableFrom(type) && !type.IsAbstract);
        }
    }
}