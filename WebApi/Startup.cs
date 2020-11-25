using System;
using System.Linq;
using Amazon.S3;
using Elasticsearch.Net;
using FluentValidation.AspNetCore;
using ImageApi.Core.Models;
using ImageApi.Core.Repositories;
using ImageApi.Infrastructure.DbContexts;
using ImageApi.Infrastructure.Options;
using ImageApi.Infrastructure.Repositories;
using ImageApi.WebApi.Utils;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;

namespace ImageApi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AwsOptions>(Configuration.GetSection("AWS"));
            services.Configure<ElasticSearchOptions>(Configuration.GetSection("ElasticSearch"));

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            var elasticSearchOpts = Configuration.GetSection("ElasticSearch").Get<ElasticSearchOptions>();
            services.AddSingleton<IElasticClient, ElasticClient>(sp =>
            {
                var pool = new StaticConnectionPool(elasticSearchOpts.URIs.Select(u => new Uri(u)));
                var settings = new ConnectionSettings(pool);
                settings.DefaultMappingFor<ImageVM>(m => m.IndexName(elasticSearchOpts.ImageIndex));

                var client = new ElasticClient(settings);

                return client;
            });

            services.AddDbContext<ImageDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ImageDbContext")));

            services.AddScoped<IImageInformationRepository, EfImageInformationRepository>();
            services.AddScoped<IImageStorageRepository, S3ImageStorageRepository>();
            services.AddScoped<IImageSearchRepository, EsImageSearchRepository>();

            services.AddMediatR(typeof(ImageApi.Core.Requests.GetImageRequest).Assembly);

            services.AddControllers().AddFluentValidation(f =>
            {
                f.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                f.RegisterValidatorsFromAssemblyContaining(typeof(ImageApi.Core.Validators.UploadImageValidator));
                f.AutomaticValidationEnabled = true;
            });

            services.AddTransient<IValidatorInterceptor, CamelCaseValidatorInterceptor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageApi v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
