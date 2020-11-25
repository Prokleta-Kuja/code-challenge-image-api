using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageApi.Core.Repositories
{
    public interface IImageStorageRepository
    {
        Task AddAsync(Guid id, Stream image);
        Task RemoveAsync(Guid id);
        Task<byte[]> GetAsync(Guid id);
    }
}