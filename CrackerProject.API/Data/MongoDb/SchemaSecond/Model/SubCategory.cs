using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class SubCategory : CourseCategory
    {
        [BsonElement("parentCategoryIds")]
        public IList<Guid> ParentCategoryIds { get; set; } = new List<Guid>();
    }
}
