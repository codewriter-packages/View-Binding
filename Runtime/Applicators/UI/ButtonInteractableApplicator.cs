using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("View Binding/UI/Button Interactable Applicator")]
    public sealed class ButtonInteractableApplicator : ComponentApplicatorBase<Button, ViewVariableBool>
    {
        [SerializeField]
        private bool inverse;

        protected override void Apply(Button target, ViewVariableBool source)
        {
            target.interactable = source.Value != inverse;
        }
    }
}