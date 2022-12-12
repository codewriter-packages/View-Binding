namespace CodeWriter.ViewBinding
{
    public delegate string LocalizationCallback(ref ValueTextBuilder textBuilder);

    public class BindingsLocalization
    {
        private static readonly LocalizationCallback DefaultCallback;

        private static LocalizationCallback _callback;

        static BindingsLocalization()
        {
            DefaultCallback = (ref ValueTextBuilder textBuilder) => textBuilder.ToString();
            _callback = DefaultCallback;
        }

        public static void SetCallback(LocalizationCallback callback)
        {
            _callback = callback ?? DefaultCallback;
        }

        public static string Localize(ref ValueTextBuilder textBuilder)
        {
            return _callback.Invoke(ref textBuilder);
        }

        public static string Localize(string text)
        {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                textBuilder.Append(text);
                return Localize(ref textBuilder);
            }
            finally
            {
                textBuilder.Dispose();
            }
        }

        public static string Localize(string text, string defaultValue)
        {
            if (_callback == DefaultCallback)
            {
                return defaultValue;
            }

            return Localize(text);
        }
    }
}