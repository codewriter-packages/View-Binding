using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ButtonClickBinder : ViewBindingBehaviour
    {
        [Required]
        [SerializeField]
        private Button button;

        [SerializeField]
        private ViewEventVoid onClick;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            button.onClick.AddLifetimedListener(lifetime, onClick.Invoke);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            button = GetComponent<Button>();
        }
#endif
    }
}