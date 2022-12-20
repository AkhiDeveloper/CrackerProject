using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
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

        [BsonElement("questions")]
        public IList<Question> Questions { get; set; } = new List<Question>();
    }
}