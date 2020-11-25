using System.Threading;
using System.Threading.Tasks;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using ImageApi.Core.Responses;
using MediatR;

namespace ImageApi.Core.Handlers
{
    public class SearchImagesHandler : IRequestHandler<SearchImagesRequest, PagedResults<ImageVM>>
    {
        private readonly IImageSearchRepository _search;

        public SearchImagesHandler(IImageSearchRepository search)
        {
            _search = search;
        }

        public async Task<PagedResults<ImageVM>> Handle(SearchImagesRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                request.Description = null;

            var results = await _search.SearchAsync(request);
            return results;
        }
    }
}