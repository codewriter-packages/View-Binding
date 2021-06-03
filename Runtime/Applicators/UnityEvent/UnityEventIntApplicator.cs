using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [AddComponentMenu("View Binding/Unity Event/UnityEvent Int Applicator")]
    public sealed class UnityEventIntApplicator : UnityEventApplicatorBase<int, ViewVariableInt>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}