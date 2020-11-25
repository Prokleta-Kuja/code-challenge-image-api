using System.Threading.Tasks;
using ImageApi.Core.Entities;

namespace ImageApi.Core.Repositories
{
    public interface IImageInformationRepository
    {
        Image Add(Image image);
        Task<Image> FindAsync(int id);
        Task SaveChangesAsync();
    }
}