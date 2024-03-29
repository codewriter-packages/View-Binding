﻿using System;
using TriInspector;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    using TMPro;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("View Binding/UI/[Binding] Formatted TMP Text Applicator")]
    public sealed class FormattedTMPTextApplicator : ApplicatorBase
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
            try
            {
                textBuilder.AppendFormat(format, extraContexts);
                target.SetText(textBuilder.RawCharArray, 0, textBuilder.Length);
            }
            finally
            {
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