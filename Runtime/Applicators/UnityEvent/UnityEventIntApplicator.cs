using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/UnityEvent Int")]
    public sealed class UnityEventIntApplicator : UnityEventApplicatorBase<int, ViewVariableInt>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}