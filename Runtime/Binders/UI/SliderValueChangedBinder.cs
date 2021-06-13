using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderValueChangedBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private ViewEventFloat onValueChanged;

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(onValueChanged.Invoke);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(onValueChanged.Invoke);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            slider = GetComponent<Slider>();
        }
#endif
    }
}