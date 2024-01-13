using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.API.Data.Models
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int SN { get; set; }

        [Required]
        public string Text { get; set; }

        public string? ImageUri { get; set; }

        [ForeignKey(nameof(QuestionSet))]
        public Guid ParentSetId { get; set; }
    }
}
