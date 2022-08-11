using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/[Binding] UnityEvent Float Applicator")]
    public sealed class UnityEventFloatApplicator : UnityEventApplicatorBase<float, ViewVariableFloat>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}