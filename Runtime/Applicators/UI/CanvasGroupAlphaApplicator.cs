using CodeWriter.ViewBinding.Applicators;
using UnityEngine;

namespace CodeWriter.ViewBinding.Extras
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("View Binding/UI/[Binding] CanvasGroup Alpha Applicator")]
    public class CanvasGroupAlphaApplicator : ComponentApplicatorBase<CanvasGroup, ViewVariableFloat>
    {
        protected override void Apply(CanvasGroup target, ViewVariableFloat source)
        {
            target.alpha = source.Value;
        }
    }
}