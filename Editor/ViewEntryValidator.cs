using TriInspector;

namespace CodeWriter.ViewBinding.Editor
{
    [RegisterTriValueValidator]
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