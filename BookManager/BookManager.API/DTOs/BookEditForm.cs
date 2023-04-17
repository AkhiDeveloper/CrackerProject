namespace BookManager.API.DTOs
{
    public class BookEditForm
    {
        public string? Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
