using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class SubCategory : Category
    {
        [BsonElement("parentCategoryIds")]
        public IList<Guid> ParentCategoryIds { get; set; } = new List<Guid>();
    }
}
