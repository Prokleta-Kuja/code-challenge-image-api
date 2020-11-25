using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Core.Repositories;
using ImageApi.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ImageApi.Infrastructure.Repositories
{
    public class EfImageInformationRepository : IImageInformationRepository
    {
        private readonly ImageDbContext _ctx;
        public EfImageInformationRepository(ImageDbContext ctx)
        {
            _ctx = ctx;
        }

        public Image Add(Image image)
        {
            _ctx.Images.Add(image);
            return image;
        }

        public async Task<Image> FindAsync(int id)
        {
            var image = await _ctx.Images.SingleOrDefaultAsync(i => i.Id == id);
            return image;
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}