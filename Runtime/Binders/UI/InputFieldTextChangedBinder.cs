using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    public class InputFieldTextChangedBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField, HideInInspector]
        private InputField inputField;

        [SerializeField]
        private ViewEventString onTextChanged;

        public override void OnContextStart()
        {
            base.OnContextStart();

            inputField.onValueChanged.AddListener(onTextChanged.Invoke);
        }

        public override void OnContextDestroy()
        {
            inputField.onValueChanged.RemoveListener(onTextChanged.Invoke);

            base.OnContextDestroy();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (inputField == null || inputField.gameObject != gameObject)
            {
                inputField = GetComponent<InputField>();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            inputField = GetComponent<InputField>();
        }
#endif
    }
}