using System;
using System.Threading;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Core.Requests;
using MediatR;

namespace ImageApi.Core.Handlers
{
    public class UploadImageHandler : IRequestHandler<UploadImageRequest, ImageVM>
    {
        private readonly IImageInformationRepository _info;
        private readonly IImageStorageRepository _storage;

        public UploadImageHandler(IImageInformationRepository info, IImageStorageRepository storage)
        {
            _info = info;
            _storage = storage;
        }

        public async Task<ImageVM> Handle(UploadImageRequest request, CancellationToken cancellationToken)
        {
            var storageId = Guid.NewGuid();
            await _storage.AddAsync(storageId, request.Image);

            try
            {
                var image = new Image(storageId, request.Description, request.Type, request.Size);
                _info.Add(image);
                await _info.SaveChangesAsync();

                var result = new ImageVM(image);

                return result;
            }
            catch (Exception)
            {
                await _storage.RemoveAsync(storageId);
                throw;
            }

        }
    }
}