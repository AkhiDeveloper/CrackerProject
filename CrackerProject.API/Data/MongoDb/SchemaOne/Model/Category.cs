using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(SubCategory), typeof(CourseCategory), typeof(MainCategory))]
    public class Category
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
