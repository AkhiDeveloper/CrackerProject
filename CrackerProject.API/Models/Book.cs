using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CrackerProject.API.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; } =DateTime.UtcNow;

        [BsonElement("publisher")]
        public string Publisher { get; set; } =string.Empty;

        [BsonElement("writer")]
        public string writer { get; set; } = string.Empty;

        [BsonElement("edition")]
        public int Edition { get; set; }

        [BsonElement("sections")]
        public IEnumerable<BookSection> Sections { get; set; }


    }
}
