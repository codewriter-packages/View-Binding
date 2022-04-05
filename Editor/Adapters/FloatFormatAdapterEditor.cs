using CodeWriter.ViewBinding.Applicators.Adapters;
using UnityEditor;

namespace CodeWriter.ViewBinding.Editor.Adapters
{
    [CustomEditor(typeof(FloatFormatAdapter))]
    public class FloatFormatAdapterEditor : SingleResultAdapterBaseEditor
    {
        private SerializedProperty _precisionProp;
        private SerializedProperty _fixedPrecisionProp;

        public override void OnEnable()
        {
            base.OnEnable();

            var resultProp = serializedObject.FindProperty("result");
            _precisionProp = resultProp.FindPropertyRelative("precision");
            _fixedPrecisionProp = resultProp.FindPropertyRelative("fixedPrecision");
        }

        protected override void DoGUI()
        {
            base.DoGUI();

            EditorGUILayout.IntSlider(_precisionProp, 0, 10);
            EditorGUILayout.PropertyField(_fixedPrecisionProp);
        }
    }
}