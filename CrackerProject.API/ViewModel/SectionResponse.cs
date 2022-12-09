
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class SectionResponse
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.Empty;

        [BsonElement("sn")]
        public int? Sn { get; set; } = null;

        [BsonElement("name")]
        [BsonRequired]
        public string? Name { get; set; } = null;

        [BsonElement("description")]
        public string? Description { get; set; } = null;

        [BsonElement("added_date")]
        [BsonRequired]
        public DateTime? AddedDate { get; set; } = null;

    }
}
