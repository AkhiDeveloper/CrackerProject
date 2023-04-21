namespace BookManager.API.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public string ImageUri { get; set; } = string.Empty;
        public IList<OptionDTO> Options { get; set; }
    }
}
