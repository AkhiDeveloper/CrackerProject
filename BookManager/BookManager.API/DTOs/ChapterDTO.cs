namespace BookManager.API.DTOs
{
    public class ChapterDTO
    {
        public Guid Id { get; set; }
        public int SN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
