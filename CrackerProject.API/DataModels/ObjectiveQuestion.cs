using CrackerProject.API.Interfaces;
using CrackerProject.API.DataModels;
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
