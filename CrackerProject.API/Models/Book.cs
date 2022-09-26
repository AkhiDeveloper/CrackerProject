using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
    }
}
