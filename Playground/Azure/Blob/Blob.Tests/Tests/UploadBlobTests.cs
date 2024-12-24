using Blob.Tests.Models;
using Playground.Shared.Extensions;

namespace Blob.Tests.Tests;

public class UploadBlobTests : BaseBlobTest
{
    [Fact]
    public async Task UploadBlobAsync()
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
        var response = await BlobContainerClient.UploadBlobAsync(filePath, data.MapToJsonStream());

        // Assert
        Assert.NotNull(response);
    }
}