namespace CodeWriter.ViewBinding
{
    public ref partial struct ValueTextBuilder
    {
        public void AppendFormat(string format, ViewContextBase context)
        {
            AppendFormat(format, new ViewContextVariablesEnumerator(context));
        }
        
        public void AppendFormat(string format, ViewContextBase context, ViewContextBase[] extraContexts)
        {
            AppendFormat(format, new ViewContextPlusArrayVariablesEnumerator(context, extraContexts));
        }

        public void AppendFormat(string format, ViewContextBase[] extraContexts)
        {
            AppendFormat(format, new ViewContextArrayVariablesEnumerator(extraContexts));
        }

        public void AppendFormat<TVariablesEnumerator>(string format, TVariablesEnumerator variables)
            where TVariablesEnumerator : IVariablesEnumerator
        {
            if (string.IsNullOrEmpty(format))
            {
                Append(format);
                return;
            }

            int prev = 0, len = format.Length, start, end;
            while (prev < len && (start = format.IndexOf('<', prev)) != -1)
            {
                Append(format, prev, start - prev);

                variables.Reset();

                while (variables.TryGetNextVariable(out var variable))
                {
                    string key;
                    if ((key = variable.Name) != null &&
                        (end = start + key.Length + 1) < len &&
                        (format[end] == '>') &&
                        (string.Compare(format, start + 1, key, 0, key.Length) == 0))
                    {
                        variable.AppendValueTo(ref this);
                        prev = end + 1;
                        goto replaced;
                    }
                }

                Append('<');
                prev = start + 1;

                replaced: ;
            }

            if (prev < format.Length)
            {
                Append(format, prev, format.Length - prev);
            }
        }
    }
}