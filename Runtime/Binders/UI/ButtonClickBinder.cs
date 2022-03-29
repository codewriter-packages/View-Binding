using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ButtonClickBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField, HideInInspector]
        private Button button;

        [SerializeField]
        private ViewEventVoid onClick;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            button.onClick.AddLifetimedListener(lifetime, onClick.Invoke);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (button == null || button.gameObject != gameObject)
            {
                button = GetComponent<Button>();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            button = GetComponent<Button>();
        }
#endif
    }
}