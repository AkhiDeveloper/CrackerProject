namespace BookManager.API.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Stream? ImageFile { get; set; }
        public int Version { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
    }
}
