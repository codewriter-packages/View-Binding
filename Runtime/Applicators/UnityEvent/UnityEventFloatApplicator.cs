using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/UnityEvent Float")]
    public sealed class UnityEventFloatApplicator : UnityEventApplicatorBase<float, ViewVariableFloat>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}