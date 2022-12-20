using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("publisher")]
        public string Publisher { get; set; } = string.Empty;

        [BsonElement("writer")]
        public string author { get; set; } = string.Empty;

        [BsonElement("edition")]
        [BsonDefaultValue(1)]
        public int Edition { get; set; }
    }
}
