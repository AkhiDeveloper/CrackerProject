using CrackerProject.API.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class QuestionSetResponse
    {
        public int? SN { get; set; } = null;

        public string? Description { get; set; } = null;

        public DateTime? AddedDate { get; set; } = null;
    }
}
