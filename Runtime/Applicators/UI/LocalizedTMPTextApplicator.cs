using System;
using TriInspector;

namespace CodeWriter.ViewBinding
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("View Binding/UI/[Binding] Localized TMP Text Applicator")]
    public class LocalizedTMPTextApplicator : ApplicatorBase
    {
        [Required]
        [SerializeField]
        private TMP_Text target;

        [Required]
        [SerializeField]
        [OnValueChanged(nameof(Apply))]
        private string format;

        [Required]
        [SerializeField]
        [ViewContextCollection]
        private ViewContextBase[] extraContexts = Array.Empty<ViewContextBase>();

        protected override void Apply()
        {
            var textBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            var localizedTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                textBuilder.AppendFormat(format, extraContexts);
                var localizedString = BindingsLocalization.Localize(ref textBuilder);
                localizedTextBuilder.AppendFormat(localizedString, extraContexts);
                target.SetText(localizedTextBuilder.RawCharArray, 0, localizedTextBuilder.Length);
            }
            finally
            {
                localizedTextBuilder.Dispose();
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

        protected override void Reset()
        {
            base.Reset();

            target = GetComponent<TMP_Text>();
        }
#endif
    }
}