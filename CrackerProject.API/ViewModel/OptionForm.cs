using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class OptionForm
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int SN { get; set; } = 1;

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("image")]
        public IFormFile? ImageFile { get; set; }

        [BsonElement("isCorrect")]
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool IsCorrect { get; set; } = false;
    }
}
