using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.DataModels
{
    public class OptionSet
    {
        [BsonId]
        [BsonElement("sn")]
        public int Sn { get; set; }

        [BsonElement("options")]
        public IEnumerable<Option> Options { get; set; }
    }
}