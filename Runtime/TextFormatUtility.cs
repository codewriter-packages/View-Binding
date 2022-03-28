using System.Text;

namespace CodeWriter.ViewBinding
{
    public static class TextFormatUtility
    {
        public static StringBuilder FormatText(string format,
            ViewContextBase context = null,
            ViewContextBase[] contexts = null)
        {
            return FormatText(format, new ViewContextProxy
            {
                single = context,
                array = contexts,
            });
        }

        private static StringBuilder FormatText(string format, ViewContextProxy contexts)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(format))
            {
                return sb;
            }

            int prev = 0, len = format.Length, start, end;
            while (prev < len && (start = format.IndexOf('<', prev)) != -1)
            {
                sb.Append(format, prev, start - prev);

                for (int contextIndex = 0, contextCount = contexts.Count; contextIndex < contextCount; contextIndex++)
                {
                    var context = contexts.Get(contextIndex);
                    if (context == null)
                    {
                        continue;
                    }

                    for (int varIndex = 0, varCount = context.VariablesCount; varIndex < varCount; varIndex++)
                    {
                        var variable = context.GetVariable(varIndex);

                        string key;
                        if ((key = variable.Name) != null &&
                            (end = start + key.Length + 1) < len &&
                            (format[end] == '>') &&
                            (string.Compare(format, start + 1, key, 0, key.Length) == 0))
                        {
                            variable.AppendValueTo(ref sb);
                            prev = end + 1;
                            goto replaced;
                        }
                    }
                }

                sb.Append('<');
                prev = start + 1;

                replaced: ;
            }

            if (prev < format.Length)
            {
                sb.Append(format, prev, format.Length - prev);
            }

            return sb;
        }

        private struct ViewContextProxy
        {
            public ViewContextBase single;
            public ViewContextBase[] array;

            public int Count => 1 + (array?.Length ?? 0);

            public ViewContextBase Get(int index)
            {
                return index == 0 ? single : array[index - 1];
            }
        }
    }
}