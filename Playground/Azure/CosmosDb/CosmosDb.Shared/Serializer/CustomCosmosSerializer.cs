using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;

namespace CosmosDb.Shared.Serializer;

public class CustomCosmosSerializer : CosmosSerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public override T FromStream<T>(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        
        using var streamReader = new StreamReader(stream);
        var json = streamReader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    public override Stream ToStream<T>(T input)
    {
        ArgumentNullException.ThrowIfNull(input);
        
        var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        
        JsonSerializer.Serialize(writer, input, _jsonSerializerOptions);
        stream.Position = 0;
        
        return stream;
    }
}
