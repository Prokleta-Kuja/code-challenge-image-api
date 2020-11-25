using System;
using ImageApi.Core.Entities;
using ImageApi.Core.Models;
using Xunit;
namespace ImageApi.Core.Tests.Models
{
    public class ImageVmTests
    {
        static readonly Guid VALID_GUID = Guid.NewGuid();
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;

        [Fact]
        public void Constructs_valid_instance()
        {
            var image = new Image(VALID_GUID, VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE);
            var vm = new ImageVM(image);
            Assert.Equal(image.Id, vm.Id);
            Assert.Equal(VALID_GUID, vm.StorageId);
            Assert.Equal(VALID_DESCRIPTION, vm.Description);
            Assert.Equal(VALID_CONTENTTYPE, vm.Type);
            Assert.Equal(VALID_SIZE, vm.Size);
        }
    }
}