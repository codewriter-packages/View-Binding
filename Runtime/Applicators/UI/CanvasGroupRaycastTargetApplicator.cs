namespace CodeWriter.ViewBinding.Extras
{
    using Applicators;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("View Binding/UI/[Binding] CanvasGroup RaycastTarget Applicator")]
    public class CanvasGroupRaycastTargetApplicator : ComponentApplicatorBase<CanvasGroup, ViewVariableBool>
    {
        [SerializeField] private bool inverse = false;

        protected override void Apply(CanvasGroup target, ViewVariableBool source)
        {
            target.blocksRaycasts = source.Value != inverse;
        }
    }
}