using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class CourseCategory : Category
    {
        [BsonElement("courseIds")]
        public IList<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}
