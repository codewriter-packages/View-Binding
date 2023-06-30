using CodeWriter.ViewBinding.Editor.Odin;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
using TriInspector.Editor.Integrations.Odin;

[assembly: RegisterValidator(typeof(OdinViewEntryValidator<>))]

namespace CodeWriter.ViewBinding.Editor.Odin
{
    public class OdinViewEntryValidator<TViewEntry> : ValueValidator<TViewEntry>
        where TViewEntry : ViewEntry
    {
#if ODIN_INSPECTOR_3_1
        public override bool CanValidateProperty(InspectorProperty property)
        {
            if (TriOdinUtility.IsDrawnByTri(property.Tree.TargetType))
            {
                return false;
            }

            return base.CanValidateProperty(property);
        }

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

        private void ValidateInternal(TViewEntry value, ValidationResult result)
        {
            var message = value.GetErrorMessage();
            if (message == null)
            {
                return;
            }

            result.Message = message;
            result.ResultType = ValidationResultType.Error;
        }
    }
}