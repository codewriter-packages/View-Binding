using UniMob;
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

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            toggle.onValueChanged.AddLifetimedListener(lifetime, onToggle.Invoke);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            toggle = GetComponent<Toggle>();
        }
#endif
    }
}