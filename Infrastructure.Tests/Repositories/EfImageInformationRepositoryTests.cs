using System;
using System.Threading.Tasks;
using ImageApi.Core.Entities;
using ImageApi.Infrastructure.DbContexts;
using ImageApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ImageApi.Infrastructure.Tests.Repositories
{
    public class EfImageInformationRepositoryTests
    {
        const string VALID_DESCRIPTION = "Description";
        const string VALID_CONTENTTYPE = "image/jpeg";
        const uint VALID_SIZE = Image.MIN_SIZE;
        const int ID = 5;

        [Fact]
        public async Task Adds_image()
        {
            var image = new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE);
            var builder = new DbContextOptionsBuilder<ImageDbContext>()
                  .UseInMemoryDatabase(databaseName: nameof(Adds_image));

            var ctx = new ImageDbContext(builder.Options);
            var sut = new EfImageInformationRepository(ctx);

            sut.Add(image);
            await sut.SaveChangesAsync();

            Assert.Single(ctx.Images);
        }

        [Fact]
        public async Task Finds_image()
        {
            var image = new Image(ID, Guid.NewGuid(), VALID_DESCRIPTION, VALID_CONTENTTYPE, VALID_SIZE);
            var builder = new DbContextOptionsBuilder<ImageDbContext>()
                  .UseInMemoryDatabase(databaseName: nameof(Finds_image));

            var ctx = new ImageDbContext(builder.Options);
            ctx.Images.Add(image);
            ctx.SaveChanges();
            var sut = new EfImageInformationRepository(ctx);

            var result = await sut.FindAsync(ID);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Finds_nothing()
        {
            var builder = new DbContextOptionsBuilder<ImageDbContext>()
                  .UseInMemoryDatabase(databaseName: nameof(Finds_nothing));

            var ctx = new ImageDbContext(builder.Options);
            var sut = new EfImageInformationRepository(ctx);

            var result = await sut.FindAsync(ID);

            Assert.Null(result);
        }
    }
}