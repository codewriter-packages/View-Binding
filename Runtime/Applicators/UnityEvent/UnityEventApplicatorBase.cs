using UnityEngine;
using UnityEngine.Events;

namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    public abstract class UnityEventApplicatorBase<TValue, TVariable> : ApplicatorBase
        where TVariable : ViewVariable
    {
        [SerializeField]
        protected internal TVariable source;

        [SerializeField]
        protected internal UnityEvent<TValue> callback;

        protected abstract void Apply();

        public sealed override void OnContextVariableChanged(ViewVariable variable)
        {
            if (source.IsLinkedTo(variable))
            {
                Apply();
#if UNITY_EDITOR
                for (int i = 0, count = callback.GetPersistentEventCount(); i < count; i++)
                {
                    UnityEditor.EditorUtility.SetDirty(callback.GetPersistentTarget(i));
                }
#endif
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Apply();
        }

        protected override void ReSubscribe()
        {
            source.Subscribe(this);
        }
    }
}