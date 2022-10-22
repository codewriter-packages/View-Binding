using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    [AddComponentMenu("View Binding/UI/[Binding] Toggle IsOn Applicator")]
    public sealed class ToggleApplicator : ComponentApplicatorBase<Toggle, ViewVariableBool>
    {
        [SerializeField] private bool inverse = false;

        protected override void Apply(Toggle target, ViewVariableBool source)
        {
            target.isOn = source.Value != inverse;
        }
    }
}