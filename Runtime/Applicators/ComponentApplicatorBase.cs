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

        protected virtual void Reset()
        {
            target = GetComponent<TTarget>();
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
        }

        protected abstract void Apply(TTarget target, TVariable source);

#if UNITY_EDITOR
        public override void OnEditorContextVariableChanged(ViewVariable variable)
        {
            if (variable.IsRootVariableFor(source))
            {
                Apply();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            source.Context.AddEditorListener(this);
        }
#endif
    }
}