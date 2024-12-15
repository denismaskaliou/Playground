using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_MongoDB.Models;

public class ProductEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    [BsonElement("name")]
    public string Name { get; set; } = null!;
 
    [BsonElement("price")]
    public int Price { get; set; }
}