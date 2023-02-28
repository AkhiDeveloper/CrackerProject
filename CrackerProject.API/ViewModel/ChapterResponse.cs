namespace CrackerProject.API.ViewModel
{
    public class ChapterResponse
    {
        public Guid Id { get; set; }
        public int Sn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
