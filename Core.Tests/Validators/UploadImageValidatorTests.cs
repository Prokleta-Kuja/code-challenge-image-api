using System.IO;
using System.Linq;
using ImageApi.Core.Entities;
using ImageApi.Core.Requests;
using ImageApi.Core.Validators;
using Xunit;
namespace ImageApi.Core.Tests.Validators
{
    public class UploadImageValidatorTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;

        [Fact]
        public void Adds_image_stream_null_error()
        {
            var sut = new UploadImageValidator();
            var request = new UploadImageRequest
            {
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                Size = VALID_SIZE
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.Image), singleErrorPropName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Adds_description_empty_error(string description)
        {
            var sut = new UploadImageValidator();
            using Stream imageStream = new MemoryStream();
            var request = new UploadImageRequest
            {
                Image = imageStream,
                Description = description,
                Type = VALID_CONTENTTYPE,
                Size = VALID_SIZE
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.Description), singleErrorPropName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Adds_empty_and_invalid_type_error(string type)
        {
            var sut = new UploadImageValidator();
            using Stream imageStream = new MemoryStream();
            var request = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Type = type,
                Size = VALID_SIZE
            };

            var result = sut.Validate(request);

            Assert.Equal(2, result.Errors.Count);
            var typeErrorCount = result.Errors.Count(e => e.PropertyName == nameof(request.Type));
            Assert.Equal(2, typeErrorCount);
        }

        [Fact]
        public void Adds_invalid_type_error()
        {
            var sut = new UploadImageValidator();
            using Stream imageStream = new MemoryStream();
            var request = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Type = "application/json",
                Size = VALID_SIZE
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.Type), singleErrorPropName);
        }

        [Theory]
        [InlineData(Image.MIN_SIZE - 1)]
        [InlineData(Image.MAX_SIZE + 1)]
        public void Adds_size_error(uint size)
        {
            var sut = new UploadImageValidator();
            using Stream imageStream = new MemoryStream();
            var request = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                Size = size
            };

            var result = sut.Validate(request);

            var singleErrorPropName = result.Errors.SingleOrDefault()?.PropertyName;
            Assert.Equal(nameof(request.Size), singleErrorPropName);
        }

        [Fact]
        public void Adds_no_errors()
        {
            var sut = new UploadImageValidator();
            using Stream imageStream = new MemoryStream();
            var request = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Type = VALID_CONTENTTYPE,
                Size = VALID_SIZE
            };

            var result = sut.Validate(request);

            Assert.Empty(result.Errors);
        }
    }
}