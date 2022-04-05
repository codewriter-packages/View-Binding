using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/UnityEvent String")]
    public sealed class UnityEventStringApplicator : UnityEventApplicatorBase<string, ViewVariableString>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}