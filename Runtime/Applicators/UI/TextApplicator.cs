using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("View Binding/UI/[Binding] Text Applicator")]
    public sealed class TextApplicator : ComponentApplicatorBase<Text, ViewVariableString>
    {
        protected override void Apply(Text target, ViewVariableString source)
        {
            target.text = source.Value;
        }
    }
}