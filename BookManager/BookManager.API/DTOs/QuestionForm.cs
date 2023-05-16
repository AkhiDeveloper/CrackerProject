namespace BookManager.API.DTOs
{
    public class QuestionForm
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string? Text { get; set; }
        public IFormFile? ImageUri { get; set; }
        public IList<OptionForm>? Options { get; set; } = new List<OptionForm>();
    }
}
