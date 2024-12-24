using Azure.Storage.Blobs.Specialized;
using Blob.Tests.Models;
using Playground.Shared.Extensions;

namespace Blob.Tests.Tests;

public class AppendBlobTests : BaseBlobTest
{
    [Fact]
    public async Task AppendBlobAsync()
    {
        // Arrange
        var fileName = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
        var filePath = $"tests/upload-blob/{fileName}.json";

        var data = new BlobDataModel
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Tags = ["tag1", "tag2"]
        };

        // Act
        var appendBlobClient = BlobContainerClient.GetAppendBlobClient(filePath);
        await appendBlobClient.CreateIfNotExistsAsync();
        await appendBlobClient.AppendBlockAsync(data.MapToJsonStream());
        await appendBlobClient.AppendBlockAsync(data.MapToJsonStream());

        // Assert
        var blobDownloadInfo = await appendBlobClient.DownloadAsync();
        var result = await blobDownloadInfo.Value.Content.MapToAsync<string>();
    }
}