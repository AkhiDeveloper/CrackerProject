using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManager.API.DTOs
{
    public class BookForm
    {
        [Required]
        public string Name { get; set; }

        
        public string? Description { get; set; } = string.Empty;

        public string? Author { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
