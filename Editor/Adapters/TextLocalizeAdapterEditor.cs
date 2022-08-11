using CodeWriter.ViewBinding.Applicators.Adapters;
using UnityEditor;

namespace CodeWriter.ViewBinding.Editor.Adapters
{
    [CustomEditor(typeof(TextLocalizeAdapter))]
    public class TextLocalizeAdapterEditor : SingleResultAdapterBaseEditor
    {
        private SerializedProperty _extraContextsProp;

        public override void OnEnable()
        {
            base.OnEnable();

            var resultProp = serializedObject.FindProperty("result");
            _extraContextsProp = resultProp.FindPropertyRelative("extraContexts");
        }

        protected override void DoGUI()
        {
            ViewContextGUI.DrawContextField(serializedObject, null, _extraContextsProp);

            base.DoGUI();
        }
    }
}