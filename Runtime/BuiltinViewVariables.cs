using System;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public sealed class ViewVariableBool : ViewVariable<bool, ViewVariableBool>
    {
        public override string TypeDisplayName => "Boolean";
    }

    [Serializable]
    public sealed class ViewVariableInt : ViewVariable<int, ViewVariableInt>
    {
        public override string TypeDisplayName => "Integer";
    }

    [Serializable]
    public sealed class ViewVariableFloat : ViewVariable<float, ViewVariableFloat>
    {
        public override string TypeDisplayName => "Float";
    }

    [Serializable]
    public sealed class ViewVariableString : ViewVariable<string, ViewVariableString>
    {
        public override string TypeDisplayName => "String";
    }
}