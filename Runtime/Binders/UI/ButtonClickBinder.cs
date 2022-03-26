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

        public override void OnContextStart()
        {
            base.OnContextStart();

            button.onClick.AddListener(onClick.Invoke);
        }

        public override void OnContextDestroy()
        {
            button.onClick.RemoveListener(onClick.Invoke);

            base.OnContextDestroy();
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