#if ODIN_INSPECTOR

using CodeWriter.ViewBinding.Editor.Odin;
using Sirenix.OdinInspector.Editor.Validation;

[assembly: RegisterValidator(typeof(OdinViewVariableValidator<>))]

namespace CodeWriter.ViewBinding.Editor.Odin
{
    public class OdinViewVariableValidator<TViewVariable> : ValueValidator<TViewVariable>
        where TViewVariable : ViewVariable
    {
        protected override void Validate(TViewVariable value, ValidationResult result)
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