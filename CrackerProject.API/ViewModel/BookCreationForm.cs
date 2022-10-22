using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class BookCreationForm
    {
        [BsonRequired]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? Edition { get; set; }
    }
}
