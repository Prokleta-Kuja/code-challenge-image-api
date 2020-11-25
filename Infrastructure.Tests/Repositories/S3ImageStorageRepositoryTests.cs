using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using ImageApi.Infrastructure.Options;
using ImageApi.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ImageApi.Infrastructure.Tests.Repositories
{
    public class S3ImageStorageRepositoryTests
    {
        private const string VALID_BUCKET_NAME = "TestImages";
        private IOptions<AwsOptions> ValidOptions
            => Microsoft.Extensions.Options.Options.Create(new AwsOptions { S3ImagesBucketName = VALID_BUCKET_NAME });

        [Fact]
        public async Task Calls_PutObjectAsnyc_with_proper_params()
        {
            var id = Guid.NewGuid();
            var s3 = new Mock<IAmazonS3>();
            var sut = new S3ImageStorageRepository(s3.Object, ValidOptions);
            using var imageStream = new MemoryStream();

            await sut.AddAsync(id, imageStream);

            s3.Verify(s => s
                .PutObjectAsync(
                    It.Is<PutObjectRequest>(por =>
                        por.BucketName == VALID_BUCKET_NAME &&
                        por.Key == id.ToString()
                        ), default),
                    Times.Once
            );
        }

        [Fact]
        public async Task Downloads_image()
        {
            var id = Guid.NewGuid();
            var bytes = Encoding.UTF8.GetBytes("Test");
            using var resultStream = new MemoryStream(bytes);

            var s3 = new Mock<IAmazonS3>();
            s3.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = resultStream
            });

            var sut = new S3ImageStorageRepository(s3.Object, ValidOptions);

            var result = await sut.GetAsync(id);

            Assert.Equal(bytes, result);
            s3.Verify(s => s
                .GetObjectAsync(
                    It.Is<string>(b => b == VALID_BUCKET_NAME),
                    It.Is<string>(k => k == id.ToString()
                    ), default),
                    Times.Once
            );
        }

        [Fact]
        public async Task Calls_DeleteObjectAsync_with_proper_params()
        {
            var id = Guid.NewGuid();
            var s3 = new Mock<IAmazonS3>();
            var sut = new S3ImageStorageRepository(s3.Object, ValidOptions);

            await sut.RemoveAsync(id);

            s3.Verify(s => s
                .DeleteObjectAsync(
                    It.Is<string>(b => b == VALID_BUCKET_NAME),
                    It.Is<string>(k => k == id.ToString()
                    ), default),
                    Times.Once
            );
        }
    }
}