using System;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UI {
    using TMPro;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("View Binding/UI/[Binding] Formatted TMP Text Applicator")]
    public sealed class FormattedTMPTextApplicator : ApplicatorBase {
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        private TMP_Text target;

        [SerializeField]
        private string format;

        [SerializeField]
        private ViewContextBase[] extraContexts = Array.Empty<ViewContextBase>();

        protected override void Apply() {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try {
                textBuilder.AppendFormat(format, extraContexts);
                target.SetText(textBuilder.RawCharArray, 0, textBuilder.Length);
            }
            finally {
                textBuilder.Dispose();
            }
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
#endif
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null || target.gameObject != gameObject)
            {
                target = GetComponent<TMP_Text>();
            }
        }

        protected override void Reset() {
            base.Reset();

            target = GetComponent<TMP_Text>();
        }
#endif
    }
}