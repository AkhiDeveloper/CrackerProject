using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Model
{
    public class OptionSet
    {
        [BsonId]
        [BsonElement("sn")]
        public int Sn { get; set; } = 1;

        [BsonElement("options")]
        public IList<Option> Options { get; set; } = new List<Option>();
    }
}