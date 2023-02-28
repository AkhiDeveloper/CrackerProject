using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class CourseChapter : Chapter
    {
        [BsonElement("parentCourseId")]
        public Guid ParentCourseId { get; set; }
    }
}
