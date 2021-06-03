using System;
using Cysharp.Text;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public sealed class ViewVariableBool : ViewVariable<bool, ViewVariableBool>
    {
        public override string TypeDisplayName => "Boolean";

        public override void AppendValueTo(ref Utf16ValueStringBuilder builder)
        {
            builder.Append(Value ? "yes" : "no");
        }
    }

    [Serializable]
    public sealed class ViewVariableInt : ViewVariable<int, ViewVariableInt>
    {
        public override string TypeDisplayName => "Integer";

        public override void AppendValueTo(ref Utf16ValueStringBuilder builder) => builder.Append(Value);
    }

    [Serializable]
    public sealed class ViewVariableFloat : ViewVariable<float, ViewVariableFloat>
    {
        public override string TypeDisplayName => "Float";

        public override void AppendValueTo(ref Utf16ValueStringBuilder builder) => builder.Append(Value);
    }

    [Serializable]
    public sealed class ViewVariableString : ViewVariable<string, ViewVariableString>
    {
        public override string TypeDisplayName => "String";
        public override void AppendValueTo(ref Utf16ValueStringBuilder builder) => builder.Append(Value);
    }
}