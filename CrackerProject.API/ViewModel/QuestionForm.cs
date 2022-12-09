using CrackerProject.API.DataModels;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CrackerProject.API.ViewModel
{
    public class QuestionForm
    {
        [Required]
        public int Sn { get; set; } = 1;

        public IFormFile? ImageFile { get; set; }

        [Required]
        public string Body { get; set; } = String.Empty;
    }
}
