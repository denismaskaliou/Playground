namespace Blob.Shared.Options;

public class BlobBaseOptions
{
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
}