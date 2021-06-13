#if ODIN_INSPECTOR

using CodeWriter.ViewBinding.Editor.Odin;
using Sirenix.OdinInspector.Editor.Validation;

[assembly: RegisterValidator(typeof(OdinViewEntryValidator<>))]

namespace CodeWriter.ViewBinding.Editor.Odin
{
    public class OdinViewEntryValidator<TViewEntry> : ValueValidator<TViewEntry>
        where TViewEntry : ViewEntry
    {
        protected override void Validate(TViewEntry value, ValidationResult result)
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

#endif