using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class Course
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; }
    }
}
