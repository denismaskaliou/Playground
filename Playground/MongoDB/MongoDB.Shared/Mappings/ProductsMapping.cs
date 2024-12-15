using MongoDB.Bson.Serialization;
using MongoDB.Shared.Entities;

namespace MongoDB.Shared.Mappings;

public static class ProductsMapping
{
    public static void Map()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
        {
            BaseEntityMappings.Map();
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Name).SetElementName("name");
                cm.MapMember(c => c.Price).SetElementName("price");
            });
        }
    }
}