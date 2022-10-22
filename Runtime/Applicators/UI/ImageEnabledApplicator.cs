using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("View Binding/UI/[Binding] Image Enabled Applicator")]
    public sealed class ImageEnabledApplicator : ComponentApplicatorBase<Image, ViewVariableBool>
    {
        [SerializeField] private bool inverse = false;

        protected override void Apply(Image target, ViewVariableBool source)
        {
            target.enabled = source.Value != inverse;
        }
    }
}