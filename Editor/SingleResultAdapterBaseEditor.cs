using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomEditor(typeof(SingleResultAdapterBase), true)]
    public class SingleResultAdapterBaseEditor : UnityEditor.Editor
    {
        private const string ResultFieldName = "result";
        private const string ExtraContextsFieldName = "extraContexts";

        private static readonly GUIContent AliasContent = new GUIContent("Alias");

        private static readonly string[] ExcludedProps =
        {
            "m_Script",
            ResultFieldName,
            ExtraContextsFieldName,
        };

        private SerializedProperty _resultProp;
        private SerializedProperty _resultNameProp;
        private SerializedProperty _extraContextsProp;

        public virtual void OnEnable()
        {
            _resultProp = serializedObject.FindProperty(ResultFieldName);
            _resultNameProp = _resultProp.FindPropertyRelative("name");
            _extraContextsProp = serializedObject.FindProperty(ExtraContextsFieldName);
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_extraContextsProp != null)
            {
                ViewContextGUI.DrawContextField(serializedObject, null, _extraContextsProp);
                GUILayout.Space(10);
            }

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