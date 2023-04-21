namespace BookManager.API.DTOs
{
    public class OptionForm
    {
        public int SN { get; set; }
        public string? Text { get; set; }
        public string? ImageUri { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}