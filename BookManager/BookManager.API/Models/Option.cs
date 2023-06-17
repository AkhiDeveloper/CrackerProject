namespace BookManager.API.Models
{
    public class Option
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public Stream? Image { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}