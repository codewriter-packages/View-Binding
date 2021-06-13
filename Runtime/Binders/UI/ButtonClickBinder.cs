using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField]
        private Button button;

        [SerializeField]
        private ViewEventVoid onClick;

        private void OnEnable()
        {
            button.onClick.AddListener(onClick.Invoke);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(onClick.Invoke);
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