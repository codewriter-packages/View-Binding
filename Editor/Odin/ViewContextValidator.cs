#if ODIN_INSPECTOR

using System.Linq;
using CodeWriter.ViewBinding.Editor.Odin;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

[assembly: RegisterValidator(typeof(ViewContextValidator))]

namespace CodeWriter.ViewBinding.Editor.Odin
{
    public class ViewContextValidator : ValueValidator<ViewContext>
    {
        public override bool CanValidateProperty(InspectorProperty property)
        {
            if (!property.IsTreeRoot)
            {
                return false;
            }

            return base.CanValidateProperty(property);
        }

#if ODIN_INSPECTOR_3_1
        protected override void Validate(ValidationResult result)
        {
            ValidateInternal(ValueEntry.SmartValue, result);
        }
#else
        protected override void Validate(TViewEntry value, ValidationResult result)
        {
            ValidateInternal(value, result);
        }
#endif

        public override RevalidationCriteria RevalidationCriteria { get; }
            = RevalidationCriteria.OnValueChangeOrChildValueChange;

        private void ValidateInternal(ViewContext value, ValidationResult result)
        {
            Debug.Log("dd");
            if (!value.Listeners.SequenceEqual(value.SearchListeners()))
            {
                result.ResultType = ValidationResultType.Error;
                result.Message = "Some listeners is missing";
                return;
            }
        }
    }
}

#endif