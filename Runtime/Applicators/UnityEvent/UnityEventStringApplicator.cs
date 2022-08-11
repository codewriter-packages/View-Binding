using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/[Binding] UnityEvent String Applicator")]
    public sealed class UnityEventStringApplicator : UnityEventApplicatorBase<string, ViewVariableString>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}