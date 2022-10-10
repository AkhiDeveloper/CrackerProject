using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class BookSection
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("added_date")]
        public DateTime AddedDate { get; set; }

        [BsonElement("sections")]
        public IEnumerable<BookSection> Sections { get; set; }

        [BsonElement("question_sets")]
        public IEnumerable<QuestionSet> QuestionSets { get; set; }

    }
}
