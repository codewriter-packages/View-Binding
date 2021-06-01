using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    public sealed class SliderValueApplicator : Applicator<Slider, ViewVariableFloat>
    {
        protected override void Apply(Slider target, ViewVariableFloat source)
        {
            target.value = source.Value;
        }
    }
}