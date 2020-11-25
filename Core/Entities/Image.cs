using System;
using System.Linq;

namespace ImageApi.Core.Entities
{
    public class Image
    {
        public const uint MIN_SIZE = 1;
        public const uint MAX_SIZE = 500 * 1024;
        public static readonly string[] ValidContentTypes = new string[] { "image/jpeg", "image/png" };
        public static readonly string InvalidContentTypeMessage = $"Supported content types: {string.Join(',', ValidContentTypes)}";
        public static readonly string InvalidSizeMessage = $"Must be between {MIN_SIZE} and {MAX_SIZE} bytes";

        internal Image(Guid storageId, string description, string type, uint size)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            if (!IsValidContentType(type))
                throw new ArgumentOutOfRangeException(nameof(type), type, InvalidContentTypeMessage);

            if (!IsValidSize(size))
                throw new ArgumentOutOfRangeException(nameof(size), size, InvalidSizeMessage);

            StorageId = storageId;
            Description = description;
            Type = type.ToLower();
            Size = size;
        }
        internal Image(int id, Guid storageId, string description, string type, uint size) : this(storageId, description, type, size)
        {
            // Only to be used in tests
            Id = id;
        }

        public int Id { get; private set; }
        public Guid StorageId { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }
        public uint Size { get; private set; }

        public static bool IsValidContentType(string contentType)
            => ValidContentTypes.Contains(contentType?.ToLower());
        public static bool IsValidSize(uint size)
            => size >= MIN_SIZE && size <= MAX_SIZE;
    }
}