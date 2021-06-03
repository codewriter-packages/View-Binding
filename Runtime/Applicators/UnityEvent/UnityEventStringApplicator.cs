using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [AddComponentMenu("View Binding/Unity Event/UnityEvent String Applicator")]
    public sealed class UnityEventStringApplicator : UnityEventApplicatorBase<string, ViewVariableString>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}