using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class BookSection : Section
    {
        [BsonElement("book_id")]
        [BsonRequired]
        public Guid? BookId { get; set; } = null;
    }
}
