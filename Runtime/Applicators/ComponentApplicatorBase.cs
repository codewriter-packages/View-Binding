using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators
{
    public abstract class ComponentApplicatorBase<TTarget, TVariable> : ApplicatorBase
        where TTarget : Component
        where TVariable : ViewVariable
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField]
        private TTarget target;

        [SerializeField]
        private TVariable source;

        public TTarget GetTarget()
        {
            return target;
        }

        protected sealed override void Apply()
        {
            if (target == null)
            {
                Debug.LogError($"Null applicator target at '{name}'", this);
                return;
            }

            if (source == null)
            {
                return;
            }

            Apply(target, source);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
#endif
        }

        protected abstract void Apply(TTarget target, TVariable source);

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            target = GetComponent<TTarget>();
        }
#endif
    }
}