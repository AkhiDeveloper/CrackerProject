using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    [BsonDiscriminator(RootClass = false)]
    [BsonKnownTypes(typeof(CourseChapter), typeof(SubChapter))]
    public class Chapter
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("sn")]
        public int Sn { get; set; } = 1;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("associatedSectionIds")]
        public IList<Guid> AssociatedSectionIds { get; set; } = new List<Guid>();
    }
}
