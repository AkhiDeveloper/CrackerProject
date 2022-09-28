using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
    }
}
