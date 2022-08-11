using CodeWriter.ViewBinding.Applicators;
using UnityEngine;

namespace CodeWriter.ViewBinding.Extras
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("View Binding/UI/[Binding] Canvas Interactable Applicator")]
    public class CanvasGroupInteractableApplicator : ComponentApplicatorBase<CanvasGroup, ViewVariableBool>
    {
        [SerializeField] private bool inverse = false;

        protected override void Apply(CanvasGroup target, ViewVariableBool source)
        {
            target.interactable = source.Value != inverse;
        }
    }
}