namespace BookManager.API.Models
{
    public class Chapter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}