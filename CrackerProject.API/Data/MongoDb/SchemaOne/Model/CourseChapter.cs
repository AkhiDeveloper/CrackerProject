using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class CourseChapter : Chapter
    {
        [BsonElement("parentCourseId")]
        public Guid ParentCourseId { get; set; }
    }
}
