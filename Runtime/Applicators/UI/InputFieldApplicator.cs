using UnityEngine;
using UnityEngine.UI;

namespace CodeWriter.ViewBinding.Applicators.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    [AddComponentMenu("View Binding/UI/InputField Applicator")]
    public sealed class InputFieldApplicator : ComponentApplicatorBase<InputField, ViewVariableString>
    {
        protected override void Apply(InputField target, ViewVariableString source)
        {
            target.text = source.Value;
        }
    }
}