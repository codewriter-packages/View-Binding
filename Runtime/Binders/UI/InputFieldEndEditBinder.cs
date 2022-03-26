using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    public class InputFieldEndEditBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField, HideInInspector]
        private InputField inputField;

        [SerializeField]
        private ViewEventString onEndEdit;

        private void OnEnable()
        {
            inputField.onEndEdit.AddListener(onEndEdit.Invoke);
        }

        private void OnDisable()
        {
            inputField.onEndEdit.RemoveListener(onEndEdit.Invoke);
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