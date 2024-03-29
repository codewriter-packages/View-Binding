using TriInspector;
using UniMob;
using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Binders.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    public class SliderValueChangedBinder : ViewBindingBehaviour
    {
        [Required]
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private ViewEventFloat onValueChanged;

        protected internal override void Setup(Lifetime lifetime)
        {
            base.Setup(lifetime);

            slider.onValueChanged.AddLifetimedListener(lifetime, onValueChanged.Invoke);
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