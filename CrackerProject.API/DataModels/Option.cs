using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class Option
    { 
        [BsonElement("sn")]
        [BsonRequired]
        public int SN { get; set; }

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("isCorrect")]
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool IsCorrect { get; set; }
    }
}