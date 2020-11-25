using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using ImageApi.WebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ImageApi.WebApi.Tests.Utils
{
    public class CamelCaseValidatorInterceptorTests
    {
        [Fact]
        public void Replaces_pascal_case_error_property_names()
        {
            var cc = new Mock<ControllerContext>();
            var vc = new Mock<IValidationContext>();
            var sut = new CamelCaseValidatorInterceptor();

            var pascalCase = "PropertyName";
            var camelCase = "propertyName";
            var valResult = new ValidationResult();
            valResult.Errors.Add(new ValidationFailure(pascalCase, "some error"));

            var result = sut.AfterMvcValidation(cc.Object, vc.Object, valResult);

            var error = result.Errors.Single();
            Assert.Equal(camelCase, error.PropertyName);
        }

        [Fact]
        public void Does_not_do_anything_beforeMvcValidation()
        {
            var cc = new Mock<ControllerContext>();
            var vc = new Mock<IValidationContext>();
            var sut = new CamelCaseValidatorInterceptor();
            var result = sut.BeforeMvcValidation(cc.Object, vc.Object);
        }
    }
}