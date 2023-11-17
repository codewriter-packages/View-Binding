using System;

namespace CodeWriter.ViewBinding.Editor
{
    internal static class ViewEntryUtils
    {
        public static Type GetValueTypeFromEntryType(Type entryType)
        {
            var type = entryType;

            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ViewEvent<,>) ||
                    type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ViewVariable<,>))
                {
                    return type.GetGenericArguments()[0];
                }

                type = type.BaseType;
            }

            return entryType;
        }
    }
}