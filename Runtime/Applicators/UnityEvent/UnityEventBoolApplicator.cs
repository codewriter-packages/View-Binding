using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [DisallowMultipleComponent]
    [AddComponentMenu("View Binding/Unity Event/[Binding] UnityEvent Bool Applicator")]
    public sealed class UnityEventBoolApplicator : UnityEventApplicatorBase<bool, ViewVariableBool>
    {
        [SerializeField]
        private bool inverse;

        protected override void Apply() => callback.Invoke(source.Value != inverse);
    }
}