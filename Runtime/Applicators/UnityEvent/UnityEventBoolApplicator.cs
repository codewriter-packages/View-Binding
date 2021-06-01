using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    public sealed class UnityEventBoolApplicator : UnityEventApplicatorBase<bool, ViewVariableBool>
    {
        [SerializeField]
        private bool inverse;

        protected override void Apply() => callback.Invoke(source.Value != inverse);
    }
}