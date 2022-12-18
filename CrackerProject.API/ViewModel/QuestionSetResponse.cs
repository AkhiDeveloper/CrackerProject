
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class QuestionSetResponse
    {
        public Guid Id { get; set; }
        public int Sn { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
