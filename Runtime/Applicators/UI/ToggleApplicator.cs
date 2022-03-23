using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [RequireComponent(typeof(Toggle))]
    [AddComponentMenu("View Binding/UI/Toggle Applicator")]
    public sealed class ToggleApplicator : ComponentApplicatorBase<Toggle, ViewVariableBool>
    {
        protected override void Apply(Toggle target, ViewVariableBool source)
        {
            target.isOn = source.Value;
        }
    }
}