using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.DataModels
{
    public class SubSection : Section
    {
        [BsonElement("parentSection_id")]
        [BsonRequired]
        public Guid? ParentSectionId { get; set; }
    }
}
