using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class Question
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("option_sets")]
        public IEnumerable<OptionSet> Optionsets { get; set; }
    }
}
