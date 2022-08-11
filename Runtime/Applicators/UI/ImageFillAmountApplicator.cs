using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("View Binding/UI/[Binding] Image FillAmount Applicator")]
    public sealed class ImageFillAmountApplicator : ComponentApplicatorBase<Image, ViewVariableFloat>
    {
        protected override void Apply(Image target, ViewVariableFloat source)
        {
            target.fillAmount = source.Value;
        }
    }
}