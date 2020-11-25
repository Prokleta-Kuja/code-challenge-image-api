using System.Linq;
using ImageApi.Core.Entities;
using ImageApi.Core.Requests;
using ImageApi.Core.Validators;
using Xunit;
namespace ImageApi.Core.Tests.Validators
{
    public class SearchImageValidatorTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_MIN_SIZE = Image.MIN_SIZE;
        const uint VALID_MAX_SIZE = Image.MAX_SIZE;

        [Fact]
        public void Adds_invalid_minSize_error()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                MinSize = VALID_MIN_SIZE - 1,
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.MinSize), singleErrorPropName);
        }

        [Fact]
        public void Adds_invalid_maxSize_error()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                MaxSize = VALID_MAX_SIZE + 1,
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.MaxSize), singleErrorPropName);
        }

        [Fact]
        public void Adds_invalid_maxSize_with_minSize_error()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                MinSize = VALID_MAX_SIZE,
                MaxSize = VALID_MIN_SIZE
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.MaxSize), singleErrorPropName);
        }

        [Fact]
        public void Adds_invalid_pageNumber_error()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                PageNumber = -1
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.PageNumber), singleErrorPropName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        public void Adds_invalid_pageSize_error(int pageSize)
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                PageSize = pageSize
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.PageSize), singleErrorPropName);
        }

        [Fact]
        public void All_fields_filled()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                MinSize = VALID_MIN_SIZE,
                MaxSize = VALID_MAX_SIZE,
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void All_fields_filled_except_description(string description)
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                Description = description,
                Type = VALID_CONTENTTYPE,
                MinSize = VALID_MIN_SIZE,
                MaxSize = VALID_MAX_SIZE,
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void All_fields_filled_except_type(string type)
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                Description = VALID_DESCRIPTION,
                Type = type,
                MinSize = VALID_MIN_SIZE,
                MaxSize = VALID_MAX_SIZE,
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void All_fields_filled_except_minSize()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                MaxSize = VALID_MAX_SIZE,
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void All_fields_filled_except_maxSize()
        {
            var sut = new SearchImageValidator();
            var request = new SearchImagesRequest
            {
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                MinSize = VALID_MIN_SIZE,
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }
    }
}