using CodeWriter.ViewBinding.Editor;
using TriInspector;

[assembly: RegisterTriValueValidator(typeof(ViewEntryValidator))]

namespace CodeWriter.ViewBinding.Editor
{
    public class ViewEntryValidator : TriValueValidator<ViewEntry>
    {
        public override TriValidationResult Validate(TriValue<ViewEntry> propertyValue)
        {
            var message = propertyValue.SmartValue.GetErrorMessage();

            return message == null
                ? TriValidationResult.Valid
                : TriValidationResult.Error(message);
        }
    }
}