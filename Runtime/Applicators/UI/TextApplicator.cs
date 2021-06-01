using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    public sealed class TextApplicator : Applicator<Text, ViewVariableString>
    {
        protected override void Apply(Text target, ViewVariableString source)
        {
            target.text = source.Value;
        }
    }
}