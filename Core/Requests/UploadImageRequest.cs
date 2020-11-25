using System.IO;
using ImageApi.Core.Models;
using MediatR;

namespace ImageApi.Core.Requests
{
    public class UploadImageRequest : IRequest<ImageVM>
    {
        public Stream Image { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public uint Size { get; set; }
    }
}