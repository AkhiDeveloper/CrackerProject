namespace BookManager.API.DTOs
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Stream? ImageFile { get; set; }
        public string? Author { get; set; }
        public int? Version { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
