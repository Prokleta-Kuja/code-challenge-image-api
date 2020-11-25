using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using ImageApi.Core.Repositories;
using ImageApi.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace ImageApi.Infrastructure.Repositories
{
    public class S3ImageStorageRepository : IImageStorageRepository
    {
        private readonly IAmazonS3 _client;
        private readonly AwsOptions _options;
        public S3ImageStorageRepository(IAmazonS3 client, IOptions<AwsOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task AddAsync(Guid id, Stream image)
        {
            var request = new PutObjectRequest();
            request.BucketName = _options.S3ImagesBucketName;
            request.InputStream = image;
            request.Key = id.ToString();
            request.CannedACL = S3CannedACL.PublicRead;

            await _client.PutObjectAsync(request);
        }

        public async Task<byte[]> GetAsync(Guid id)
        {
            var response = await _client.GetObjectAsync(_options.S3ImagesBucketName, id.ToString());
            using var ms = new MemoryStream();
            response.ResponseStream.CopyTo(ms);

            return ms.ToArray();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _client.DeleteObjectAsync(_options.S3ImagesBucketName, id.ToString());
        }
    }
}