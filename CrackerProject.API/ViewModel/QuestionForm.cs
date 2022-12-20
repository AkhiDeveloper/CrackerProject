
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CrackerProject.API.ViewModel
{
    public class QuestionForm
    {
        [Required]
        public int Sn { get; set; } = 1;

        [Required]
        public string Body { get; set; } = String.Empty;
    }
}
