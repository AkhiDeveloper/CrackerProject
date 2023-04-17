namespace BookManager.API.Models
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
        public string ImageUri { get; set; } = string.Empty;
        public IList<Option> Options { get; set; } = new List<Option>();
    }
}
