using System;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Models;
using ImageApi.Core.Requests;
using ImageApi.Infrastructure.Repositories;
using Moq;
using Nest;
using Xunit;

namespace ImageApi.Infrastructure.Tests.Repositories
{
    public class EsImageSearchRepositoryTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;
        const int ID = 5;

        [Fact]
        public async Task Calls_IndexDocumentAsync()
        {
            var es = new Mock<IElasticClient>();
            var sut = new EsImageSearchRepository(es.Object);
            var image = new ImageVM(
                new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE)
            );

            await sut.AddAsync(image);

            es.Verify(s => s
                .IndexDocumentAsync<ImageVM>(
                    It.IsAny<ImageVM>(), default),
                    Times.Once
            );
        }

        [Fact]
        public async Task Calls_SearchAsync_without_filters()
        {
            var res = new Mock<ISearchResponse<ImageVM>>();
            res.Setup(r => r.Documents).Returns(new ImageVM[0]);

            var es = new Mock<IElasticClient>();
            es.Setup(e => e.SearchAsync<ImageVM>(It.IsAny<ISearchRequest>(), default))
              .ReturnsAsync(res.Object);

            var sut = new EsImageSearchRepository(es.Object);
            var request = new SearchImagesRequest();
            var image = new ImageVM(
                new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE)
            );

            await sut.SearchAsync(request);

            es.Verify(s => s
                .SearchAsync<ImageVM>(
                    It.IsAny<ISearchRequest>(), default),
                    Times.Once
            );
        }

        [Fact]
        public async Task Calls_SearchAsync_with_filters()
        {
            var res = new Mock<ISearchResponse<ImageVM>>();
            res.Setup(r => r.Documents).Returns(new ImageVM[0]);

            var es = new Mock<IElasticClient>();
            es.Setup(e => e.SearchAsync<ImageVM>(It.IsAny<ISearchRequest>(), default))
              .ReturnsAsync(res.Object);

            var sut = new EsImageSearchRepository(es.Object);
            var request = new SearchImagesRequest
            {
                Description = "test",
                Type = "image/png",
                MinSize = 1,
                MaxSize = 500,
            };
            var image = new ImageVM(
                new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE)
            );

            await sut.SearchAsync(request);

            es.Verify(s => s
                .SearchAsync<ImageVM>(
                    It.IsAny<ISearchRequest>(), default),
                    Times.Once
            );
        }
    }
}