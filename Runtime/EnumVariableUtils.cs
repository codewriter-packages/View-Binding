using System.Collections.Generic;
using System.Linq;

namespace CodeWriter.ViewBinding
{
    internal static class EnumVariableUtils
    {
        private static readonly Dictionary<string, string[]> ValuesCache = new Dictionary<string, string[]>();

        public static bool TryGetEnumValues(string variableName, out string[] values)
        {
            var start = variableName.IndexOf('[');
            if (start == -1)
            {
                values = default;
                return false;
            }

            var end = variableName.IndexOf(']', start);
            if (end == -1)
            {
                values = default;
                return false;
            }

            if (!ValuesCache.TryGetValue(variableName, out values))
            {
                values = variableName
                    .Substring(start + 1, end - start - 1)
                    .Split(',')
                    .Select(it => it.Trim())
                    .ToArray();

                ValuesCache.Add(variableName, values);
            }

            return true;
        }
    }
}