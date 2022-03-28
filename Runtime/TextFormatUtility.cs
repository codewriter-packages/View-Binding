using System.Text;

namespace CodeWriter.ViewBinding
{
    public static class TextFormatUtility
    {
        public static void FormatText(StringBuilder output, string format,
            ViewContextBase context = null,
            ViewContextBase[] contexts = null)
        {
            output.Clear();

            if ((context == null && contexts == null) || string.IsNullOrEmpty(format))
            {
                output.Append(format);
                return;
            }

            FormatText(output, format, new ViewContextProxy
            {
                single = context,
                array = contexts,
            });
        }

        private static void FormatText(StringBuilder output, string format, ViewContextProxy contexts)
        {
            int prev = 0, len = format.Length, start, end;
            while (prev < len && (start = format.IndexOf('<', prev)) != -1)
            {
                output.Append(format, prev, start - prev);

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
                            variable.AppendValueTo(ref output);
                            prev = end + 1;
                            goto replaced;
                        }
                    }
                }

                output.Append('<');
                prev = start + 1;

                replaced: ;
            }

            if (prev < format.Length)
            {
                output.Append(format, prev, format.Length - prev);
            }
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