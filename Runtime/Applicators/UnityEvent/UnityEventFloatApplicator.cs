using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [AddComponentMenu("View Binding/Unity Event/UnityEvent Float Applicator")]
    public sealed class UnityEventFloatApplicator : UnityEventApplicatorBase<float, ViewVariableFloat>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}