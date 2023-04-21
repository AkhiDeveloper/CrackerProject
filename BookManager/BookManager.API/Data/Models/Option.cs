using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.API.Data.Models
{
    public class Option
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int SN { get; set; }

        public string? Text { get; set; }

        public string? ImageUri { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Question))]
        [Required]
        public Guid QuestionId { get; set; }
    }
}
