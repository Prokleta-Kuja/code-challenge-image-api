using ImageApi.Core.Models;
using MediatR;

namespace ImageApi.Core.Requests
{
    public class GetImageRequest : IRequest<ImageVM>
    {
        public int Id { get; set; }
    }
}