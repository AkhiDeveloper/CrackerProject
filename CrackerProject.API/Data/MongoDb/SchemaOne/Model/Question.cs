using CrackerProject.API.Data.MongoDb.SchemaOne.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class Question
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("sn")]
        [BsonRequired]
        public int Sn { get; set; } = 1;

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; } = string.Empty;

        [BsonElement("imageUri")]
        public string? ImageUri { get; set; } = null;

        [BsonElement("created_date")]
        [BsonRequired]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [BsonElement("option_sets")]
        [BsonRequired]
        public IList<OptionSet> OptionSets { get; set; } = new List<OptionSet>();
    }
}
