using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("View Binding/UI/Slider Value")]
    public sealed class SliderValueApplicator : ComponentApplicatorBase<Slider, ViewVariableFloat>
    {
        protected override void Apply(Slider target, ViewVariableFloat source)
        {
            target.value = source.Value;
        }
    }
}