namespace CrackerProject.API.Model.Course
{
    public class Chapter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Sn { get; set; } = 1;
        public string Name { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
