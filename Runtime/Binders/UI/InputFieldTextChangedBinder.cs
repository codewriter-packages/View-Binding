using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    public class InputFieldTextChangedBinder : ViewBindingBehaviour
    {
        [Required]
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private ViewEventString onTextChanged;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            inputField.onValueChanged.AddLifetimedListener(lifetime, onTextChanged.Invoke);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            inputField = GetComponent<InputField>();
        }
#endif
    }
}