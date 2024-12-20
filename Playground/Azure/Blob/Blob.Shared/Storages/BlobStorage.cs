using System.Text;
using Azure.Storage.Blobs;
using Blob.Shared.Options;

namespace Blob.Shared.Storages;

public class BlobStorage(BlobBaseOptions options): IBlobStorage
{
    public async Task UploadAsync(string fileName, string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var containerClient = new BlobContainerClient(options.ConnectionString, options.ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobClient.UploadAsync(stream, overwrite: true);
    }
}