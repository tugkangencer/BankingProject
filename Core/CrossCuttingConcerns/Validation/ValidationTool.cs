using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static IList<ValidationFailure> Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);

            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                return result.Errors;
            }
            return null;
        }
    }
}
