using CrackerProject.API.DataModels;
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
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [BsonElement("questions")]
        public IList<Question> Questions { get; set; } = new List<Question>();

        [BsonElement("objective_questions")]
        public IList<ObjectiveQuestion> ObjectiveQuestions { get; set; } = new List<ObjectiveQuestion>();
    }
}