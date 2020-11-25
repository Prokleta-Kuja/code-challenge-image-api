using System;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Handlers;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using Moq;
using Xunit;
namespace ImageApi.Core.Tests.Handlers
{
    public class GetImageHandlerTestsHandlerTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;
        const int ID = 5;
        static Image GenerateImage =>
            new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE);

        [Fact]
        public async Task Returns_Found()
        {
            var cmd = new GetImageRequest { Id = ID };
            var info = new Mock<IImageInformationRepository>();
            info.Setup(i => i.FindAsync(It.Is<int>(id => id == cmd.Id)))
                .ReturnsAsync(GenerateImage);

            var sut = new GetImageHandler(info.Object);
            var result = await sut.Handle(cmd, default);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Returns_null()
        {
            var cmd = new GetImageRequest { Id = default };
            var info = new Mock<IImageInformationRepository>();
            info.Setup(i => i.FindAsync(It.Is<int>(id => id == cmd.Id)))
                .ReturnsAsync((Image)null);

            var sut = new GetImageHandler(info.Object);
            var result = await sut.Handle(cmd, default);

            Assert.Null(result);
        }
    }
}