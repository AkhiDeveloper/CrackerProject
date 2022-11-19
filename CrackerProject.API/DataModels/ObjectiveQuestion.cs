using CrackerProject.API.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.DataModels
{
    public class ObjectiveQuestion : Question
    {
        [BsonElement("option_sets")]
        [BsonRequired]
        public IList<OptionSet> OptionSets { get; set; } = new List<OptionSet>();
    }
}
