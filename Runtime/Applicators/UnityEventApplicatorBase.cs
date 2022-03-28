using UnityEngine;
using UnityEngine.Events;

namespace CodeWriter.ViewBinding.Applicators
{
    public abstract class UnityEventApplicatorBase<TValue, TVariable> : ApplicatorBase
        where TVariable : ViewVariable
    {
        [SerializeField]
        protected internal TVariable source;

        [SerializeField]
        protected internal UnityEvent<TValue> callback;

#if UNITY_EDITOR
        public sealed override void OnEditorContextVariableChanged(ViewVariable variable)
        {
            if (variable.IsRootVariableFor(source))
            {
                Apply();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            this.EditorTrackModificationsOf(source);
        }
#endif
    }
}