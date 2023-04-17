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

        [Required]
        [StringLength(80, MinimumLength = 1, ErrorMessage = "Book Name should be between characters of 3 and 80")]
        public string Text { get; set; }

        public string ImageUri { get; set; } = string.Empty;

        public bool IsCorrect { get; set; } = false;

        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
    }
}
