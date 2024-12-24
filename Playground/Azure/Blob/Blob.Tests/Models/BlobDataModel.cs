namespace Blob.Tests.Models;

public class BlobDataModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public IList<string> Tags { get; set; } = null!;
}