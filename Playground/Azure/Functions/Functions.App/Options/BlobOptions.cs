using Blob.Shared.Options;

namespace Functions.App.Options;

public class BlobOptions : BlobBaseOptions
{
    public const string SectionName = "Blob";

    public string OrdersStorageName { get; set; } = default!;
}