using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class OptionResponse
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int SN { get; set; }

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("imageUri")]
        public string? ImageUri { get; set; }

        [BsonElement("isCorrect")]
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool IsCorrect { get; set; }
    }
}