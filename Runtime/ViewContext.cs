using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        internal override int VariablesCount => vars.Count;

        internal override ViewVariable GetVariable(int index) => vars[index];

        protected void UnsafeRegisterVariable(ViewVariable variable)
        {
            vars.Add(variable);
        }

        public ViewVariable FindVariable(string variableName)
        {
            foreach (var variable in vars)
            {
                if (variable.Name == variableName)
                {
                    return variable;
                }
            }

            return null;
        }

        public TViewVariable FindVariable<TViewVariable>(string variableName)
            where TViewVariable : ViewVariable
        {
            foreach (var variable in vars)
            {
                if (variable.Name == variableName && variable is TViewVariable tVariable)
                {
                    return tVariable;
                }
            }

            return null;
        }

        public StringBuilder FormatText(string format)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(format))
            {
                return sb;
            }

            if (VariablesCount == 0)
            {
                return sb;
            }

            int prev = 0, len = format.Length, start, end;
            while (prev < len && (start = format.IndexOf('<', prev)) != -1)
            {
                sb.Append(format, prev, start - prev);

                for (int i = 0, varCount = VariablesCount; i < varCount; i++)
                {
                    var variable = GetVariable(i);

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
    }
}