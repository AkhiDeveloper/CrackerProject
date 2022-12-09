namespace CrackerProject.API.Model
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; } = String.Empty;

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public string Publisher { get; set; } = string.Empty;

        public string author { get; set; } = string.Empty;

        public int Edition { get; set; }
    }
}
