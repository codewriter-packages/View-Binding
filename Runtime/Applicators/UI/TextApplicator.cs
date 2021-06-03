using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("View Binding/UI/Text Applicator")]
    public sealed class TextApplicator : Applicator<Text, ViewVariableString>
    {
        protected override void Apply(Text target, ViewVariableString source)
        {
            target.text = source.Value;
        }
    }
}