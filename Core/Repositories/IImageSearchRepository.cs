using System.Threading.Tasks;
using ImageApi.Core.Models;
using ImageApi.Core.Requests;
using ImageApi.Core.Responses;

namespace ImageApi.Core.Repositories
{
    public interface IImageSearchRepository
    {
        Task AddAsync(ImageVM image);
        Task<PagedResults<ImageVM>> SearchAsync(SearchImagesRequest req);
    }
}