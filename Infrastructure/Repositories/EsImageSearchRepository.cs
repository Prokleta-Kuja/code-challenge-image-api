using System.Threading.Tasks;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using ImageApi.Core.Responses;
using Nest;

namespace ImageApi.Infrastructure.Repositories
{
    public class EsImageSearchRepository : IImageSearchRepository
    {
        private readonly IElasticClient _client;

        public EsImageSearchRepository(IElasticClient client)
        {
            _client = client;
        }

        public async Task AddAsync(ImageVM image)
        {
            await _client.IndexDocumentAsync(image);
        }

        public async Task<PagedResults<ImageVM>> SearchAsync(SearchImagesRequest req)
        {
            var request = new SearchDescriptor<ImageVM>()
                .From(req.PageNumber * req.PageSize)
                .Size(req.PageSize)
                .Query(q =>
                    q.Match(m => m.Field(f => f.Description).Query(req.Description))
                    && q.Match(m => m.Field(f => f.Type).Query(req.Type))
                    && q.Range(r => r.Field(f => f.Size)
                        .GreaterThanOrEquals(req.MinSize)
                        .LessThanOrEquals(req.MaxSize))
            );

            var response = await _client.SearchAsync<ImageVM>(request);
            var results = new PagedResults<ImageVM>(req.PageNumber, req.PageSize, response.Documents);
            return results;
        }
    }
}