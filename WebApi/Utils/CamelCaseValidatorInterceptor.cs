using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ImageApi.WebApi.Utils
{
    public class CamelCaseValidatorInterceptor : IValidatorInterceptor
    {
        internal static string GetCamelCase(string key)
        {
            char[] a = key.ToCharArray();
            a[0] = char.ToLower(a[0]);

            return new string(a);
        }
        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext, ValidationResult result)
        {
            foreach (var error in result.Errors)
                error.PropertyName = GetCamelCase(error.PropertyName);

            return result;
        }

        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
            => commonContext;
    }
}