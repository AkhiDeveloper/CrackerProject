using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class OptionForm
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int Sn { get; set; } = 1;

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("isCorrect")]
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool IsCorrect { get; set; } = false;
    }
}
