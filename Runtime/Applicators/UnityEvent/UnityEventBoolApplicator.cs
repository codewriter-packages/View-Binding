using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    [AddComponentMenu("View Binding/Unity Event/UnityEvent Bool Applicator")]
    public sealed class UnityEventBoolApplicator : UnityEventApplicatorBase<bool, ViewVariableBool>
    {
        [SerializeField]
        private bool inverse;

        protected override void Apply() => callback.Invoke(source.Value != inverse);
    }
}