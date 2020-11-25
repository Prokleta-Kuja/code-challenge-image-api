using System.Linq;
using ImageApi.Core.Repositories;
using ImageApi.Infrastructure.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;

namespace ImageApi.WebApi.Tests
{
    public class ImageApiFactory : WebApplicationFactory<Startup>
    {
        internal Mock<IMediator> Mediator = new Mock<IMediator>();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var logger = services.SingleOrDefault(d => d.ServiceType == typeof(ILogger));
                services.Remove(logger);

                var esClient = services.SingleOrDefault(d => d.ServiceType == typeof(IElasticClient));
                services.Remove(esClient);
                services.AddScoped<IElasticClient>(s => new Mock<IElasticClient>().Object);

                var dbCtx = services.SingleOrDefault(d => d.ServiceType == typeof(ImageDbContext));
                services.Remove(dbCtx);
                services.AddScoped<ImageDbContext>(s => new Mock<ImageDbContext>().Object);

                var info = services.SingleOrDefault(d => d.ServiceType == typeof(IImageInformationRepository));
                services.Remove(info);
                services.AddScoped<IImageInformationRepository>(s => new Mock<IImageInformationRepository>().Object);

                var storage = services.SingleOrDefault(d => d.ServiceType == typeof(IImageStorageRepository));
                services.Remove(storage);
                services.AddScoped<IImageStorageRepository>(s => new Mock<IImageStorageRepository>().Object);

                var search = services.SingleOrDefault(d => d.ServiceType == typeof(IImageSearchRepository));
                services.Remove(search);
                services.AddScoped<IImageSearchRepository>(s => new Mock<IImageSearchRepository>().Object);

                var mediator = services.SingleOrDefault(d => d.ServiceType == typeof(IMediator));
                services.Remove(mediator);
                services.AddTransient<IMediator>(f => Mediator.Object);
            });
        }
    }
}