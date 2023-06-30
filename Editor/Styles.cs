using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    internal static class Styles
    {
        public static readonly GUIContent NoneContent = GUIContent.none;

        public static readonly GUIStyle RedBoldLabel;

        static Styles()
        {
            RedBoldLabel = new GUIStyle(EditorStyles.whiteBoldLabel)
            {
                normal =
                {
                    textColor = Color.red,
                }
            };
        }
    }
}