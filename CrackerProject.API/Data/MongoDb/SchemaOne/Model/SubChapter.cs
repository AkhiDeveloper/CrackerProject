using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class SubChapter : Chapter
    {
        [BsonElement("parentChapterId")]
        public Guid ParentChapterId { get; set; }
    }
}
