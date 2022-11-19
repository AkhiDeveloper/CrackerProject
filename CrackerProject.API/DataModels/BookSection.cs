using CrackerProject.API.DataModels;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class BookSection : Section
    {
        [BsonElement("book_id")]
        [BsonRequired]
        public Guid? BookId { get; set; } = null;
    }
}
