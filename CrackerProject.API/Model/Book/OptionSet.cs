using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model.Book
{
    public class OptionSet
    {
        public int Sn { get; set; }
        public IList<Option> Options { get; set; } = new List<Option>();
    }
}
