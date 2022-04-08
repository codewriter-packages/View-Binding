using CodeWriter.ViewBinding.Applicators.Adapters;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor.Adapters
{
    [CustomEditor(typeof(TextLocalizeAdapter))]
    public class TextLocalizeAdapterEditor : SingleResultAdapterBaseEditor
    {
        private SerializedProperty _extraContextsProp;
        private ReorderableList _extraContextsList;

        public override void OnEnable()
        {
            base.OnEnable();

            var resultProp = serializedObject.FindProperty("result");
            _extraContextsProp = resultProp.FindPropertyRelative("extraContexts");

            _extraContextsList = new ReorderableList(serializedObject, _extraContextsProp)
            {
                drawHeaderCallback = rect => GUI.Label(rect, "Extra Contexts"),
                elementHeightCallback = index => EditorGUIUtility.singleLineHeight,
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var prop = _extraContextsProp.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, prop);
                }
            };
        }

        protected override void DoGUI()
        {
            base.DoGUI();

            _extraContextsList.DoLayoutList();
        }
    }
}