using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class OptionSet
    {
        [BsonId]
        [BsonElement("sn")]
        public int Sn { get; set; }

        [BsonElement("options")]
        public IEnumerable<Option> Options { get; set; }
    }
}