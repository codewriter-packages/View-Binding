#if UNIMOB_UI_TMPRO

using UnityEngine;
using TMPro;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("View Binding/UI/TMP Text")]
    public sealed class TMPTextApplicator : ComponentApplicatorBase<TMP_Text, ViewVariableString>
    {
        protected override void Apply(TMP_Text target, ViewVariableString source)
        {
            target.text = source.Value;
        }
    }
}

#endif