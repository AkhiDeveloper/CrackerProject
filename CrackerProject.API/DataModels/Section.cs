using CrackerProject.API.DataModels;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.DataModels
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(BookSection) , typeof(SubSection))]
    public class Section
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("sn")]
        public int Sn { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("added_date")]
        [BsonRequired]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [BsonElement("questionSets")]
        public IList<QuestionSet> QuestionSets { get; set; } = new List<QuestionSet>();
    }
}
