using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class Applicator<TTarget, TVariable> : ApplicatorBase
        where TTarget : Component
        where TVariable : ViewVariable
    {
        [SerializeField]
        private TTarget target;

        [SerializeField]
        private TVariable source;

        protected virtual void Reset()
        {
            target = GetComponent<TTarget>();
        }

        protected abstract void Apply(TTarget target, TVariable source);

        public sealed override void OnContextVariableChanged(ViewVariable variable)
        {
            if (source.IsLinkedTo(variable))
            {
                Apply(target, source);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(target);
#endif
            }
        }

        protected sealed override void ReSubscribe()
        {
            source.Subscribe(this);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Apply(target, source);
        }
    }
}