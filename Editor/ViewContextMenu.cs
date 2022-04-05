using UnityEditor;

namespace CodeWriter.ViewBinding.Editor
{
    public class ViewContextMenu
    {
        private const string ShowRuntimeValuesPath = "CONTEXT/ViewContext/Show Runtime Values";

        public static bool ShowRuntimeValues
        {
            get => EditorPrefs.GetBool("ViewBinding_ShowRuntimeValues", true);
            set => EditorPrefs.SetBool("ViewBinding_ShowRuntimeValues", value);
        }

        [InitializeOnLoadMethod]
        private static void RefreshMenu()
        {
            Menu.SetChecked(ShowRuntimeValuesPath, ShowRuntimeValues);
        }

        [MenuItem(ShowRuntimeValuesPath)]
        private static void ToggleShowRuntimeValues(MenuCommand command)
        {
            ShowRuntimeValues = !ShowRuntimeValues;

            RefreshMenu();
        }
    }
}