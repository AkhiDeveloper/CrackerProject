using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class QuestionSet
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("sn")]
        public int Sn { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("added_date")]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [BsonElement("questionIds")]
        public IList<Guid> QuestionIds { get; set; } = new List<Guid>();
    }
}