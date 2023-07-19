namespace BookManager.API.DTOs
{
    public class OptionDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public Stream? Image { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}
