using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Web.Code.Extensions;

public static class ValidationExtensions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors) modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }
}