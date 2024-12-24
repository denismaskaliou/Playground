using System.Text;
using Newtonsoft.Json;

namespace Playground.Shared.Extensions;

public static class StreamExtensions
{
    public static MemoryStream MapToJsonStream<T>(this T  data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var jsonString = JsonConvert.SerializeObject(data);
        var byteArray = Encoding.UTF8.GetBytes(jsonString);
        return new MemoryStream(byteArray);
    }

    public static async Task<T?> MapToAsync<T>(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var reader = new StreamReader(stream, Encoding.UTF8);
        var downloadedJson = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<T>(downloadedJson);
    }
}