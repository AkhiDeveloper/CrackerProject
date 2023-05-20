namespace BookManager.API.DTOs
{
    public class QuestionForm
    {
        public int SN { get; set; }
        public string? Text { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
