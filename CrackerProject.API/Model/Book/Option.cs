using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model.Book
{
    public class Option
    {
        public int Sn { get; set; }

        public string Body { get; set; }

        public string? ImagePath { get; set; }

        public bool IsCorrect { get; set; }
    }
}
