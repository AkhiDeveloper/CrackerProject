using CrackerProject.API.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class ObjectiveQuestionResponse : QuestionResponse
    {
        [BsonElement("option_sets")]
        [BsonRequired]
        public IList<OptionSetResponse> OptionSets { get; set; } = new List<OptionSetResponse>();
    }
}
