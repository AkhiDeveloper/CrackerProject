using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class OptionSet
    {
        [BsonElement("sn")]
        public int SN { get; set; }

        [BsonElement("options")]
        public IEnumerable<Option> Options { get; set; }

        [BsonElement("correct_opton_sn")]
        public int CorrectOptionSN { get; set; }
    }
}