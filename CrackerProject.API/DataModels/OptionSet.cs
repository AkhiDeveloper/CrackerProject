using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class OptionSet
    {
        [BsonId]
        [BsonElement("sn")]
        public int SN { get; set; }

        [BsonElement("options")]
        public IEnumerable<Option> Options { get; set; }
    }
}