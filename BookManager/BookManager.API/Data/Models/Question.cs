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
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Book Name should be between characters of 3 and 80")]
        public string Text { get; set; }

        public string? ImageUri { get; set; }

        [ForeignKey(nameof(QuestionSet))]
        public Guid ParentSetId { get; set; }
    }
}
