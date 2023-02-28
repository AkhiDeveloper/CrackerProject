using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class SubSection : Section
    {
        [BsonElement("parentSection_id")]
        [BsonRequired]
        public Guid? ParentSectionId { get; set; }
    }
}
