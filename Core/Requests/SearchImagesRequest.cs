using ImageApi.Core.Models;
using ImageApi.Core.Responses;
using MediatR;

namespace ImageApi.Core.Requests
{
    public class SearchImagesRequest : IRequest<PagedResults<ImageVM>>
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public uint? MinSize { get; set; }
        public uint? MaxSize { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 20;
    }
}