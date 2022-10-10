using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class QuestionSet
    {
        [BsonElement("sn")]
        public int SN { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("added_date")]
        public DateTime AddedDate { get; set; }

        [BsonElement("question_ids")]
        public IEnumerable<string> QuestionIds { get; set; }
    }
}