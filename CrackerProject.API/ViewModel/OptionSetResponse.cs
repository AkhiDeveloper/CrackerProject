using CrackerProject.API.DataModels;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class OptionSetResponse
    {
        [BsonId]
        [BsonElement("sn")]
        public int SN { get; set; }

        [BsonElement("options")]
        public IEnumerable<OptionResponse> Options { get; set; }
    }
}
