namespace Blob.Shared.Storages;

public interface IBlobStorage
{
    Task UploadAsync(string fileName, string content);
}