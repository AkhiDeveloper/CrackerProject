using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model.Book
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Sn { get; set; } = 1;

        public string Body { get; set; } = string.Empty;

        public string? ImagePath { get; set; } = null;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public IList<OptionSet> OptionSets { get; set; } = new List<OptionSet>();
    }
}
