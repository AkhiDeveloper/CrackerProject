using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model
{
    public class OptionSet
    {
        public int Sn { get; set; }
        public IEnumerable<Option> Options { get; set; }
    }
}
