using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class Question
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int Sn { get; set; }

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("created_date")]
        [BsonRequired]
        public DateTime CreatedDate { get; set; }

        [BsonElement("option_set_Ids")]
        public OptionSet[] OptionSets { get; set; }
    }
}
