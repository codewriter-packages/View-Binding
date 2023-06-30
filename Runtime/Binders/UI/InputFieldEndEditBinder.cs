using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    public class InputFieldEndEditBinder : ViewBindingBehaviour
    {
        [Required]
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private ViewEventString onEndEdit;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            inputField.onEndEdit.AddLifetimedListener(lifetime, onEndEdit.Invoke);
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