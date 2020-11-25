using System.Threading.Tasks;
using Xunit;
using Moq;
using ImageApi.Core.Requests;
using ImageApi.Core.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace ImageApi.WebApi.Tests.Controllers
{
    public class ImagesControllerTests : IClassFixture<ImageApiFactory>
    {
        private readonly ImageApiFactory _factory;
        private static readonly byte[] SomeBytes = Encoding.UTF8.GetBytes("Test");

        public ImagesControllerTests(ImageApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetById_Returns_404()
        {
            _factory.Mediator.Setup(m => m.Send(It.IsAny<GetImageRequest>(), default))
                             .ReturnsAsync((ImageVM)null);

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/images/5");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_Returns_200()
        {
            _factory.Mediator.Setup(m => m.Send(It.IsAny<GetImageRequest>(), default))
                             .ReturnsAsync(new ImageVM(new Core.Entities.Image(Guid.NewGuid(), "a", "image/png", 5)));

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/images/5");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_Returns_400()
        {
            _factory.Mediator.Setup(m => m.Send(It.IsAny<UploadImageRequest>(), default))
                             .ThrowsAsync(new Exception());

            var client = _factory.CreateClient();
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(new byte[0]);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/invalid");
            form.Add(fileContent, "file", "test.png");
            form.Add(new StringContent(string.Empty), "description");

            // Act
            var response = await client.PostAsync($"api/images/", form);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_Returns_201()
        {
            _factory.Mediator.Setup(m => m.Send(It.IsAny<UploadImageRequest>(), default))
                             .ReturnsAsync(new ImageVM(new Core.Entities.Image(Guid.NewGuid(), "a", "image/png", 5)));

            var client = _factory.CreateClient();
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(SomeBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
            form.Add(fileContent, "file", "test.png");
            form.Add(new StringContent("test"), "description");

            // Act
            var response = await client.PostAsync($"api/images/", form);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}