using System.Linq;
using System.Reflection;
using UnityEditor;

namespace CodeWriter.ViewBinding.Editor
{
    internal static class ScriptAttributeUtilityProxy
    {
        private static readonly MethodProxy GetFieldInfoAndStaticTypeFromPropertyMethod;

        static ScriptAttributeUtilityProxy()
        {
            var unityEditorAssemblyTypes = typeof(UnityEditor.Editor).Assembly.GetTypes();

            GetFieldInfoAndStaticTypeFromPropertyMethod = new MethodProxy
            {
                methodInfo = unityEditorAssemblyTypes
                    .First(t => t.Name == "ScriptAttributeUtility")
                    .GetMethod("GetFieldInfoAndStaticTypeFromProperty", BindingFlags.Static | BindingFlags.NonPublic),
                parameters = new object[2],
            };
        }

        public static FieldInfo GetFieldInfoAndStaticTypeFromProperty(
            SerializedProperty property,
            out System.Type type)
        {
            var proxy = GetFieldInfoAndStaticTypeFromPropertyMethod;

            proxy.parameters[0] = property;
            proxy.parameters[1] = null;

            var result = proxy.methodInfo.Invoke(null, proxy.parameters);

            type = (System.Type) proxy.parameters[1];

            proxy.parameters[0] = null;
            proxy.parameters[1] = null;

            return (FieldInfo) result;
        }

        private class MethodProxy
        {
            public MethodInfo methodInfo;
            public object[] parameters;
        }
    }
}