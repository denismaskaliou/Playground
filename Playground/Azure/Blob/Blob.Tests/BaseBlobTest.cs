using Azure.Storage.Blobs;

namespace Blob.Tests;

public abstract class BaseBlobTest
{
    private const string ContainerName = "tests-container";
    private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=playgroundstaccount;AccountKey=aKAFRo+fAdcy+kHcBuAbfW3vbPsoT13EWW5mRsucVllcGUa6b1VTPIQZpRM6eSb+f4dtqRxt1i3r+ASt2vfioA==;EndpointSuffix=core.windows.net";

    protected BlobContainerClient BlobContainerClient { get; } = CreateBlobContainerClient();

    private static BlobContainerClient CreateBlobContainerClient()
    {
        var blobContainerClient = new BlobContainerClient(ConnectionString, ContainerName);
        blobContainerClient.CreateIfNotExists();
        return blobContainerClient;
    }
}