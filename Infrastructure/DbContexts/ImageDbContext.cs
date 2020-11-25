using ImageApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageApi.Infrastructure.DbContexts
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options) { }

        public DbSet<Image> Images { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.StorageId).IsRequired();
                e.Property(p => p.Description).IsRequired();
                e.Property(p => p.Type).IsRequired();
            });
        }
    }
}