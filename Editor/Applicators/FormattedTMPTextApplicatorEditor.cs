using CodeWriter.ViewBinding.Applicators.UI;
using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor.Applicators
{
    [CustomEditor(typeof(FormattedTMPTextApplicator))]
    public class FormattedTMPTextApplicatorEditor : UnityEditor.Editor
    {
        private const string TargetPropName = "target";
        private const string FormatPropName = "format";
        private const string ExtraContextsPropName = "extraContexts";

        private static readonly string[] ExcludedProperties =
        {
            "m_Script",
            TargetPropName,
            FormatPropName,
            ExtraContextsPropName
        };

        private SerializedProperty _formatProp;
        private SerializedProperty _extraContextsProp;

        private void OnEnable()
        {
            _formatProp = serializedObject.FindProperty(FormatPropName);
            _extraContextsProp = serializedObject.FindProperty(ExtraContextsPropName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ViewContextGUI.DrawContextField(serializedObject, null, _extraContextsProp);

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_formatProp);

            if (EditorGUI.EndChangeCheck())
            {
                if (target is FormattedTMPTextApplicator applicator)
                {
                    serializedObject.ApplyModifiedProperties();
                    applicator.Apply();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}