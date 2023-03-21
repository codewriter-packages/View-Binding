using System;

namespace CodeWriter.ViewBinding
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("View Binding/UI/[Binding] Localized TMP Text Applicator")]
    public class LocalizedTMPTextApplicator : ApplicatorBase
    {
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        private TMP_Text target;

        [SerializeField]
        private string format;

        [SerializeField]
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