using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class BookSection : Section
    {
        [BsonElement("book_id")]
        [BsonRequired]
        public Guid? BookId { get; set; } = null;
    }
}
