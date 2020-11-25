using System.Threading;
using System.Threading.Tasks;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using MediatR;

namespace ImageApi.Core.Handlers
{
    public class GetImageHandler : IRequestHandler<GetImageRequest, ImageVM>
    {
        private readonly IImageInformationRepository _info;

        public GetImageHandler(IImageInformationRepository info)
        {
            _info = info;
        }

        public async Task<ImageVM> Handle(GetImageRequest request, CancellationToken cancellationToken)
        {
            var image = await _info.FindAsync(request.Id);
            if (image != null)
                return new ImageVM(image);

            return null;
        }
    }
}