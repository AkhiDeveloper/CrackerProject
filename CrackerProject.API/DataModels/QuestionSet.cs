using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class QuestionSet
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int SN { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("added_date")]
        public DateTime AddedDate { get; set; }

        [BsonElement("questions")]
        public Question[] questions { get; set; }
    }
}