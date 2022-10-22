namespace CodeWriter.ViewBinding.Extras
{
    using Applicators;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("View Binding/UI/[Binding] CanvasGroup Visibility Applicator")]
    public class CanvasGroupVisibilityApplicator : ComponentApplicatorBase<CanvasGroup, ViewVariableBool>
    {
        [SerializeField] private bool inverse = false;

        protected override void Apply(CanvasGroup target, ViewVariableBool source)
        {
            var visible = source.Value != inverse;

            target.blocksRaycasts = visible;
            target.interactable = visible;
            target.alpha = visible ? 1 : 0;
        }
    }
}