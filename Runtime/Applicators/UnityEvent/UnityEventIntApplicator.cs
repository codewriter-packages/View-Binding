using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/[Binding] UnityEvent Int Applicator")]
    public sealed class UnityEventIntApplicator : UnityEventApplicatorBase<int, ViewVariableInt>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}