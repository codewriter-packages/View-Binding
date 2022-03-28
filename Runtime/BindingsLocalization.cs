using System;
using System.Text;

namespace CodeWriter.ViewBinding
{
    public class BindingsLocalization
    {
        private static readonly Func<StringBuilder, string> DefaultCallback;

        private static Func<StringBuilder, string> _callback;

        static BindingsLocalization()
        {
            DefaultCallback = sb => sb.ToString();
            _callback = DefaultCallback;
        }

        public static void SetCallback(Func<StringBuilder, string> callback)
        {
            _callback = callback ?? DefaultCallback;
        }

        public static string Localize(StringBuilder stringBuilder)
        {
            return _callback.Invoke(stringBuilder);
        }
    }
}