using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class ToggleValueChangedBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField, HideInInspector]
        private Toggle toggle;

        [SerializeField]
        private ViewEventBool onToggle;

        public override void OnContextStart()
        {
            base.OnContextStart();

            toggle.onValueChanged.AddListener(onToggle.Invoke);
        }

        public override void OnContextDestroy()
        {
            toggle.onValueChanged.RemoveListener(onToggle.Invoke);

            base.OnContextDestroy();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (toggle == null || toggle.gameObject != gameObject)
            {
                toggle = GetComponent<Toggle>();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            toggle = GetComponent<Toggle>();
        }
#endif
    }
}