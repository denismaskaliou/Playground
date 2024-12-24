using Azure.Storage.Blobs.Specialized;
using Blob.Tests.Models;
using Playground.Shared.Extensions;

namespace Blob.Tests.Tests;

public class DownloadBlobTests : BaseBlobTest
{
    [Fact]
    public async Task DownloadBlobAsync()
    {
        // Arrange
        var fileName = "tests/upload-blob/2024-12-23-15-53-13.json";

        // Act
        var blobClient = BlobContainerClient.GetBlobClient(fileName);
        var blobDownloadInfo = await blobClient.DownloadAsync();

        var data = await blobDownloadInfo.Value.Content.MapToAsync<BlobDataModel>();

        // Assert
        var response = blobDownloadInfo.GetRawResponse();
        Assert.NotNull(blobDownloadInfo);
    }
}