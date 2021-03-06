using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomEditor(typeof(SingleResultAdapterBase), true)]
    public class SingleResultAdapterBaseEditor : UnityEditor.Editor
    {
        private const string ResultFieldName = "result";

        private static readonly GUIContent AliasContent = new GUIContent("Alias");

        private static readonly string[] ExcludedProps =
        {
            "m_Script",
            ResultFieldName
        };

        private SerializedProperty _resultProp;
        private SerializedProperty _resultNameProp;

        public virtual void OnEnable()
        {
            _resultProp = serializedObject.FindProperty(ResultFieldName);
            _resultNameProp = _resultProp.FindPropertyRelative("name");
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            DoGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DoGUI()
        {
            EditorGUILayout.PropertyField(_resultNameProp, AliasContent);

            DrawPropertiesExcluding(serializedObject, ExcludedProps);
        }
    }
}