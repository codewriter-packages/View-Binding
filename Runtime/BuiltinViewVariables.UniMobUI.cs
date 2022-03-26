#if UNIMOB_UI

using System;
using System.Text;
using UniMob.UI;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public sealed class ViewVariableState : ViewVariable<IState, ViewVariableState>
    {
        public override string TypeDisplayName => "UI State";
        public override void AppendValueTo(ref StringBuilder builder) => builder.Append("[UI State]");
    }
}

#endif