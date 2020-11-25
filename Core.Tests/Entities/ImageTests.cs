using System;
using ImageApi.Core.Entities;
using Xunit;
namespace ImageApi.Core.Tests.Entities
{
    public class ImageTests
    {
        static readonly Guid VALID_GUID = Guid.NewGuid();
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;

        [Fact]
        public void Throws_when_description_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Image(VALID_GUID, null, VALID_CONTENTTYPE, VALID_SIZE));
        }

        [Fact]
        public void Throws_when_description_is_empty()
        {
            Assert.Throws<ArgumentNullException>(() => new Image(VALID_GUID, string.Empty, VALID_CONTENTTYPE, VALID_SIZE));
        }

        [Fact]
        public void Throws_when_size_is_smaller()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Image(VALID_GUID, VALID_DESCRIPTION, VALID_CONTENTTYPE, Image.MIN_SIZE - 1));
        }

        [Fact]
        public void Throws_when_size_is_greater()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Image(VALID_GUID, VALID_DESCRIPTION, VALID_CONTENTTYPE, Image.MAX_SIZE + 1));
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("")]
        [InlineData(null)]
        public void Throws_when_invalid_contenttype(string contentType)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Image(VALID_GUID, VALID_DESCRIPTION, contentType, VALID_SIZE));
        }

        [Fact]
        public void Makes_valid_type_lowercase()
        {
            var image = new Image(VALID_GUID, VALID_DESCRIPTION, "iMage/jPeg", VALID_SIZE);
            Assert.Equal(VALID_CONTENTTYPE, image.Type);
        }

        [Fact]
        public void Constructs_valid_instance()
        {
            var image = new Image(VALID_GUID, VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE);
            Assert.Equal(0, image.Id);
            Assert.Equal(VALID_GUID, image.StorageId);
            Assert.Equal(VALID_DESCRIPTION, image.Description);
            Assert.Equal(VALID_CONTENTTYPE, image.Type);
            Assert.Equal(VALID_SIZE, image.Size);
        }
    }
}