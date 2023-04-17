using System.ComponentModel.DataAnnotations;

namespace BookManager.API.Data.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(80, MinimumLength = 3,ErrorMessage = "Book Name should be between characters of 3 and 80")]
        public string Name { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? Author { get; set; } = string.Empty;

        public int Version { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;
    }
}
