using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    public class SliderValueChangedBinder : ViewBindingBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField, HideInInspector]
        private Slider slider;

        [SerializeField]
        private ViewEventFloat onValueChanged;

        public override void OnContextStart()
        {
            base.OnContextStart();

            slider.onValueChanged.AddListener(onValueChanged.Invoke);
        }

        public override void OnContextDestroy()
        {
            slider.onValueChanged.RemoveListener(onValueChanged.Invoke);

            base.OnContextDestroy();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (slider == null || slider.gameObject != gameObject)
            {
                slider = GetComponent<Slider>();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            slider = GetComponent<Slider>();
        }
#endif
    }
}