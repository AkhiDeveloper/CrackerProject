namespace BookManager.API.Models
{
    public class QuestionSet
    {
        Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string? Description { get; set; }
    }
}
