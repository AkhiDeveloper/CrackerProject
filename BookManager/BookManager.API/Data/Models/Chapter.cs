using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.API.Data.Models
{
    public class Chapter
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int SN { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Book Name should be between characters of 3 and 80")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [ForeignKey(nameof(Book))]
        [Required]
        public Guid BookId { get; set; }

        [ForeignKey(nameof(Chapter))]
        public Guid? ParentChapterId { get; set; }
    }
}