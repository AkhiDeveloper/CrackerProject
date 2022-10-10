using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class Option
    { 
        [BsonElement("sn")]
        public int SN { get; set; }

        [BsonElement("body")]
        public string Body { get; set; } = String.Empty;
    }
}