using System;
using ImageApi.Core.Entities;

namespace ImageApi.Core.Models
{
    public class ImageVM
    {
        public ImageVM() { }
        internal ImageVM(Image image)
        {
            Id = image.Id;
            StorageId = image.StorageId;
            Description = image.Description;
            Type = image.Type;
            Size = image.Size;
        }

        public int Id { get; set; }
        public Guid StorageId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public uint Size { get; set; }
    }
}