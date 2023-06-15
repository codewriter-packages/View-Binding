using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class ToggleValueChangedBinder : ViewBindingBehaviour
    {
        [Required]
        [SerializeField]
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