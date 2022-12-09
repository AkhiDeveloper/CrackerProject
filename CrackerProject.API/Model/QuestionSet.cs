
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model
{
    public class QuestionSet
    {
        public Guid Id { get; set; }

        public int QuestionSetSN { get; set; }

        public string Description { get; set; } = String.Empty;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}
