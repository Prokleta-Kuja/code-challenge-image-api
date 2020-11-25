using System;
using System.IO;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Handlers;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using Moq;
using Xunit;
namespace ImageApi.Core.Tests.Handlers
{
    public class UploadImageHandlerTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;

        [Fact]
        public async Task Throws_if_add_to_storage_fails()
        {
            var info = new Mock<IImageInformationRepository>();
            var storage = new Mock<IImageStorageRepository>();
            storage
                .Setup(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()))
                .ThrowsAsync(It.IsAny<Exception>());

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Never);
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task Throws_if_add_to_info_fails()
        {
            var storage = new Mock<IImageStorageRepository>();
            var info = new Mock<IImageInformationRepository>();
            info
                .Setup(i => i.Add(It.IsAny<Image>()))
                .Throws(It.IsAny<Exception>());

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task Throws_if_info_save_fails()
        {
            var storage = new Mock<IImageStorageRepository>();
            var info = new Mock<IImageInformationRepository>();
            info
                .Setup(i => i.SaveChangesAsync())
                .ThrowsAsync(It.IsAny<Exception>());

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
            info.Verify(i => i.SaveChangesAsync(), Times.Once);
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task Removes_from_storage_if_add_to_info_fails()
        {
            var storage = new Mock<IImageStorageRepository>();
            var info = new Mock<IImageInformationRepository>();
            info
                .Setup(i => i.Add(It.IsAny<Image>()))
                .Throws(It.IsAny<Exception>());

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
            storage.Verify(s => s.RemoveAsync(It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task Removes_from_storage_if_info_save_fails()
        {
            var storage = new Mock<IImageStorageRepository>();
            var info = new Mock<IImageInformationRepository>();
            info
                .Setup(i => i.SaveChangesAsync())
                .ThrowsAsync(It.IsAny<Exception>());

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
            info.Verify(i => i.SaveChangesAsync(), Times.Once);
            storage.Verify(s => s.RemoveAsync(It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task Returns_response()
        {
            var storage = new Mock<IImageStorageRepository>();
            var info = new Mock<IImageInformationRepository>();

            using Stream imageStream = new MemoryStream();
            var cmd = new UploadImageRequest
            {
                Image = imageStream,
                Description = VALID_DESCRIPTION,
                Size = VALID_SIZE,
                Type = VALID_CONTENTTYPE
            };

            var sut = new UploadImageHandler(info.Object, storage.Object);
            var image = await sut.Handle(cmd, default);

            storage.Verify(s => s.AddAsync(It.IsAny<Guid>(), It.IsAny<Stream>()), Times.Once);
            info.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
            info.Verify(i => i.SaveChangesAsync(), Times.Once);
            storage.Verify(s => s.RemoveAsync(It.IsAny<Guid>()), Times.Never);

            Assert.NotEqual(Guid.Empty, image.StorageId);
            Assert.Equal(VALID_DESCRIPTION, image.Description);
            Assert.Equal(VALID_CONTENTTYPE, image.Type);
            Assert.Equal(VALID_SIZE, image.Size);
        }
    }
}