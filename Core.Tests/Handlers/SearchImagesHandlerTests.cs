using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Handlers;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using ImageApi.Core.Responses;
using Moq;
using Xunit;
namespace ImageApi.Core.Tests.Handlers
{
    public class SearchImagesHandlerTests
    {
        const string VALID_CONTENTTYPE = "image/jpeg";
        static IEnumerable<ImageVM> GenerateImages(int count) => Enumerable.Range(0, count)
                                                                    .Select(i => new Image(Guid.NewGuid(), $"Description {i}", VALID_CONTENTTYPE, 1))
                                                                    .Select(i => new ImageVM(i));

        [Fact]
        public async Task Throws_if_search_fails()
        {
            var search = new Mock<IImageSearchRepository>();
            search.Setup(s => s.SearchAsync(It.IsAny<SearchImagesRequest>()))
                  .ThrowsAsync(It.IsAny<Exception>());
            var cmd = new SearchImagesRequest();

            var sut = new SearchImagesHandler(search.Object);
            var ex = await Record.ExceptionAsync(() => sut.Handle(cmd, default));

            search.Verify(s => s.SearchAsync(cmd), Times.Once);
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Sets_description_null_if_empty_or_whitespace(string description)
        {
            var search = new Mock<IImageSearchRepository>();
            search.Setup(s => s.SearchAsync(It.IsAny<SearchImagesRequest>()))
                  .ReturnsAsync(new PagedResults<ImageVM>(0, 20, GenerateImages(20)));
            var cmd = new SearchImagesRequest { Description = description };

            var sut = new SearchImagesHandler(search.Object);
            var result = await sut.Handle(cmd, default);

            search.Verify(s => s.SearchAsync(It.Is<SearchImagesRequest>(c => c.Description == null)), Times.Once);
        }

        [Fact]
        public async Task Returns_results()
        {
            var search = new Mock<IImageSearchRepository>();
            search.Setup(s => s.SearchAsync(It.IsAny<SearchImagesRequest>()))
                  .ReturnsAsync(new PagedResults<ImageVM>(0, 20, GenerateImages(20)));
            var cmd = new SearchImagesRequest();

            var sut = new SearchImagesHandler(search.Object);
            var result = await sut.Handle(cmd, default);

            search.Verify(s => s.SearchAsync(cmd), Times.Once);
            Assert.NotEmpty(result.Results);
        }
    }
}