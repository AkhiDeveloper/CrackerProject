using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model
{
    public class OptionSet
    {
        public IEnumerable<Option> Options { get; set; }
    }
}
