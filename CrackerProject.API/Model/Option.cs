using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model
{
    public class Option
    {
        public int SN { get; set; }

        public string Body { get; set; }

        public string? ImagePath { get; set; }

        public bool IsCorrect { get; set; }
    }
}
